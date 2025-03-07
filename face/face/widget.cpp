#include "widget.h"
#include "ui_widget.h"
#include "stdio.h"
#include <iostream>

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);

    // 获取可用的摄像头设备列表，并在UI的listWidget中显示摄像头描述
    list = QCameraInfo::availableCameras();
    for(auto ele:list){
        ui->listWidget->addItem(ele.description());
    }

    // 加载人脸特征库文件
    classifier = new cv::CascadeClassifier("D:/STD/QT_opencv/etc/haarcascades/haarcascade_frontalface_default.xml");
    if(classifier->empty()){
        QMessageBox::warning(this, "加载haar特征库", "特征库加载失败");
    }

    // 设置画笔颜色和宽度
    pen.setColor(QColor(0, 255, 0));
    pen.setWidth(5);

    // 连接并打开数据库，若失败则输出错误信息
    db = QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName("opencv.db");
    if(!db.open()) {
        qDebug() << "数据库打开失败";
    } else {
        query = new QSqlQuery(db);
        // 创建人脸表格，若不存在则创建
        query->prepare("create table if not exists face(label integer primary key, name text not null)");
        bool res = query->exec();
        if(!res){
            qDebug() << query->lastError();
        }
    }
}

Widget::~Widget()
{
    delete ui;
}

// 将OpenCV的Mat对象转换为QPixmap用于显示
QPixmap Widget::matToPix(cv::Mat mat)
{
    QImage image(mat.data, mat.cols, mat.rows, mat.cols * mat.channels(), QImage::Format_Grayscale8);
    return QPixmap::fromImage(image);
}

// 双击摄像头列表项时，选择对应摄像头
void Widget::on_listWidget_itemDoubleClicked(QListWidgetItem *item)
{
    int row = ui->listWidget->currentRow();
    QCameraInfo info = list[row];

    camera = new QCamera(info, this);

    abs = new Abs(this);  // 创建图像处理对象

    // 将Abs对象的图像信号与Widget的图像接收函数连接
    QObject::connect(abs, &Abs::sndImage, this, &Widget::rcvImage);

    // 将摄像头捕获的画面交由Abs处理
    camera->setViewfinder(abs);

    QMessageBox::information(this, "选择摄像头", "选择摄像头成功");
}

// 判断是否有人脸匹配成功
bool Widget::hasFace(cv::Mat face)
{
    QFile file("D:/face/face/face/face.xml");
    if(!file.exists()){
        return false;
    }

    // 加载已保存的人脸识别器模型
    recognizer = cv::face::LBPHFaceRecognizer::load<cv::face::LBPHFaceRecognizer>("D:/QT_opencv/src/face.xml");
    double confidence = -1;
    int face_label = -1;
    
    // 进行人脸匹配预测，返回匹配标签和置信度
    recognizer->predict(face, face_label, confidence);

    // 若置信度低于120，则认为人脸匹配成功
    return confidence < 120.0;
}

// 接收摄像头捕获的图像，进行处理并显示
void Widget::rcvImage(QImage image)
{
    // 调整摄像头图片大小以适应label_2
    image = image.scaled(ui->label_2->size());
    QPixmap pic = QPixmap::fromImage(image);

    // 将QImage转换为OpenCV的Mat格式，用于后续的人脸检测
    cv::Mat mat(image.height(), image.width(), CV_8UC4, (void*)image.bits());

    // 将彩色图像转换为灰度图，以提高人脸检测效率
    cv::Mat gray_mat;
    cv::cvtColor(mat, gray_mat, CV_BGR2GRAY);

    // 进行人脸检测
    classifier->detectMultiScale(gray_mat, rects, 1.2, 6, -1, {50, 50}, {300, 300});

    // 使用画笔绘制检测到的人脸矩形框
    painter.begin(&pic);
    painter.setPen(pen);
    if (!rects.empty()) {
        painter.drawRect(rects[0].x, rects[0].y, rects[0].width, rects[0].height);
    }
    painter.end();

    // 根据选择执行人脸录入或匹配操作
    if(choice != 0){
        QRect face_rect(rects[0].x, rects[0].y, rects[0].width, rects[0].height);
        count++;
        QImage face_image = image.copy(face_rect);
        cv::Mat face_mat(face_image.height(), face_image.width(), CV_8UC4, (void*)face_image.bits());
        cv::Mat gray_face_mat;
        cv::cvtColor(face_mat, gray_face_mat, CV_BGR2GRAY);

        // 人脸录入逻辑
        if(choice == 1 && count >= 20){
            query->prepare("select label from face order by label desc limit 1");
            if (!query->exec()) {
                qDebug() << query->lastError();
            }

            int label = 0;
            if(query->next()) {
                label = query->value("label").toInt();
            }

            // 将捕获到的人脸图像存入人脸容器
            faces.push_back(gray_face_mat);
            face_labels.push_back(label + 1);

            QFile file("D:/QT_opencv/src/face.xml");
            if(file.exists()){
                // 如果已有模型，加载并更新
                recognizer = cv::face::LBPHFaceRecognizer::load<cv::face::LBPHFaceRecognizer>("D:/face/face/face/face.xml");
            } else {
                // 如果没有模型，则创建新的
                recognizer = cv::face::LBPHFaceRecognizer::create();
            }

            // 检查人脸是否已经存在
            if(hasFace(gray_face_mat)){
                qDebug() << "当前人脸已存在";
                choice = 0;
                count = 0;
            } else {
                // 更新人脸库并保存
                recognizer->update(faces, face_labels);
                recognizer->save("D:/face/face/face/face.xml");
                choice = 0;
                count = 0;
                faces.clear();
                face_labels.clear();
                qDebug() << "人脸录入成功";

                // 将新的人脸信息存入数据库
                QString name = ui->lineEdit->text();
                query->prepare("insert into face(label, name) values(:label, :name)");
                query->bindValue(":label", label + 1);
                query->bindValue(":name", name);
                query->exec();
            }
        } else if(choice == 2){
            // 人脸匹配逻辑
            recognizer = cv::face::LBPHFaceRecognizer::load<cv::face::LBPHFaceRecognizer>("D:/face/face/face/face.xml");
            double confidence = -1;
            int face_label = -1;
            recognizer->predict(gray_face_mat, face_label, confidence);
            choice = 0;

            qDebug() << "confidence = " << confidence;
            if(confidence >= 120.0){
                qDebug() << "没有找到匹配的人脸";
            } else {
                // 找到匹配的人脸，通过标签在数据库中查询对应的姓名
                query->prepare("select name from face where label = :label");
                query->bindValue(":label", face_label);
                query->exec();
                query->next();
                QString name = query->value("name").toString();
                qDebug() << "人脸登录成功，用户名为:" + name;
            }
        }
    }

    // 将处理后的图像显示在label_2上
    ui->label_2->setPixmap(pic);
}

// 停止摄像头
void Widget::on_pushButton_2_clicked()
{
    camera->stop();
}

// 启动摄像头
void Widget::on_pushButton_clicked()
{
    camera->start();
}

// 开始人脸录入
void Widget::on_pushButton_3_clicked()
{
    choice = 1;
}

// 开始人脸匹配
void Widget::on_pushButton_4_clicked()
{
    choice = 2;
}

// 删除数据库中的人脸表单和人脸模型文件
void Widget::on_pushButton_5_clicked()
{
    query->exec("delete from face");
    QFile file("D:/face/face/face/face.xml");
    file.close();
    file.remove();
    query->exec("drop table face");
}
