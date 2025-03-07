#ifndef ABS_H
#define ABS_H

#include <QAbstractVideoSurface>
class Abs : public QAbstractVideoSurface
{
    Q_OBJECT
public:
    Abs(QObject* parent =nullptr);

    virtual bool present(const QVideoFrame &frame);

    virtual QList<QVideoFrame::PixelFormat> supportedPixelFormats(QAbstractVideoBuffer::HandleType type = QAbstractVideoBuffer::NoHandle) const;


signals:
    void sndImage(QImage image);

};

#endif // ABS_H
