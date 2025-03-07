#include "abs.h"

Abs::Abs(QObject* parent)
    :QAbstractVideoSurface(parent)
{

}

bool Abs::present(const QVideoFrame &frame)
{
    // 复制视频帧，避免修改原始数据
    QVideoFrame fm = frame;
    
    // 映射视频帧的存储空间，确保可以访问数据
    fm.map(QAbstractVideoBuffer::ReadOnly);
    
    // 使用视频帧的数据构造 QImage
    QImage image(fm.bits(), fm.width(), fm.height(), QImage::Format_RGB32);
    
    // 翻转图像，因为摄像头捕获的图像是镜像的
    image = image.mirrored(true, true);
    
    // 发送图像信号，供其他组件使用
    emit sndImage(image);
}

// 返回支持的视频帧像素格式，这里仅支持 RGB32 格式
QList<QVideoFrame::PixelFormat> Abs::supportedPixelFormats(QAbstractVideoBuffer::HandleType type) const
{
    QList<QVideoFrame::PixelFormat> list;
    list << QVideoFrame::Format_RGB32;
    return list;
}