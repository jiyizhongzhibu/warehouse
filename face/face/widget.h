#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QCamera>
#include <QCameraInfo>
#include <QListWidget>
#include "abs.h"
#include <QMessageBox>
#include <opencv/highgui.h>
#include <opencv2/opencv.hpp>
#include <opencv2/face.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/objdetect.hpp>
#include <opencv2/face.hpp>
#include <opencv/cv.h>
#include <vector>
#include <opencv2/opencv.hpp>
#include <QPainter>
#include <QPen>
#include <QSqlDatabase>
#include <QSqlQuery>
#include <QSqlError>
#include <QFile>
/*#include <D:/QT_opencv/include/opencv2/opencv.hpp>
#include <opencv2/opencv.hpp>
*/


QT_BEGIN_NAMESPACE
namespace Ui { class Widget; }
QT_END_NAMESPACE

class Widget : public QWidget
{
    Q_OBJECT

public:
    Widget(QWidget *parent = nullptr);
    ~Widget();
    QPixmap matToPix(cv::Mat mat);
    bool hasFace(cv::Mat face);


private slots:
    void on_listWidget_itemDoubleClicked(QListWidgetItem *item);
private slots:
    void rcvImage(QImage image);
    void on_pushButton_clicked();

    void on_pushButton_2_clicked();

    void on_pushButton_3_clicked();

    void on_pushButton_4_clicked();

    void on_pushButton_5_clicked();

private:
    Ui::Widget *ui;
    QCamera* camera;
    QList<QCameraInfo> list;
    Abs* abs;
    cv::CascadeClassifier* classifier;
    std::vector<cv::Rect> rects;
    cv::Ptr<cv::face::LBPHFaceRecognizer> recognizer;
    QPen pen;
    QPainter painter;
    std::vector<cv::Mat> faces;
    std::vector<int> face_labels;
    int choice =0;
    int count =0;

    QSqlDatabase db;
    QSqlQuery* query;
};

#endif // WIDGET_H

