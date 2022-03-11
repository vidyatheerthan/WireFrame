using Windows.Foundation;

namespace WireFrame.DrawArea
{
    public interface IBox
    {
        double GetLeft();
        void SetLeft(double left);

        double GetTop();
        void SetTop(double top);

        double GetLength();
        void SetLength(double length);

        double GetBreath();
        void SetBreath(double breath);

        void GetScale(ref double x, ref double y);
        void SetScale(double x, double y);

        void SetTransformOrigin(Point point);
        Point GetTransformOrigin();

        double GetRotation();
        void SetRotation(double rotation);
    }
}
