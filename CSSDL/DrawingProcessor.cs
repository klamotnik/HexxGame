using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public class DrawingProcessor
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct SurfaceStruct
        {
            public UInt32 Flags { get; }
            public IntPtr Format { get; }
            public int Width { get; }
            public int Height { get; }
            public int Pitch { get; }
            public IntPtr Pixels { get; }
            public IntPtr UserData { get; }
            public int Locked { get; }
            public IntPtr LockData { get; }
            public Rectangle ClipRectangle { get; }
            public IntPtr Map { get; }
            public int ReferenceCount { get; }
        }

        public Surface Surface { get; private set; }
        private SurfaceStruct surfaceStruct;

        public DrawingProcessor(Surface surface)
        {
            Surface = surface;
            surfaceStruct = (SurfaceStruct)Marshal.PtrToStructure(Surface.handle, typeof(SurfaceStruct));
        }

        public Color GetPixel(int x, int y)
        {
            int rgba = Marshal.ReadInt32(surfaceStruct.Pixels, (x + y * Surface.Properties.w) * 4);
            return new Color()
            {
                r = (byte)((rgba & 0xFF)),
                g = (byte)((rgba & 0xFF00) >> 8),
                b = (byte)((rgba & 0xFF0000) >> 16),
                a = (byte)((rgba & 0xFF000000) >> 24)
            };
        }

        public void SetPixel(int x, int y, Color color)
        {
            Marshal.WriteInt32(surfaceStruct.Pixels, (x + y * Surface.Properties.w) * 4, (int)color.ToUint());
        }

        public void DrawLine(int x1, int y1, int x2, int y2, int size, Color color)
        {
            SetPixel(x1, y1, color);
            SetPixel(x2, y2, color);
            
            int x = x1, y = y1;
            float dx = x2 - x1, dy = y2 - y1, d = 0, kx = 1, ky = 1;
            if (dx < 0)
            {
                dx = x1 - x2;
                kx = -1;
            }
            if (dy < 0)
            {
                dy = y1 - y2;
                ky = -1;
            }
            SetPixel(x, y, color);
            if (dy <= dx)
            {
                d = 2 * dy - dx;
                while (x != x2)
                {
                    if (d >= 0)
                    {
                        d = d + 2 * (dy - dx);
                        y += (int)ky;
                    }
                    else
                        d = d + 2 * dy;
                    x += (int)kx;
                    SetPixel(x, y, color);
                }
            }
            else
            {
                d = 2 * dx - dy;
                while (y != y2)
                {

                    if (d >= 0)
                    {
                        d = d + 2 * (dx - dy);
                        x += (int)kx;
                    }
                    else
                        d = d + 2 * dx;
                    y += (int)ky;

                    SetPixel(x, y, color);

                }
            }
        }

        public void DrawRoundedBorder(Color color)
        {
            DrawLine(2, 0, Surface.Properties.w - 3, 0, 1, color);
            DrawLine(2, Surface.Properties.h - 1, Surface.Properties.w - 3, Surface.Properties.h - 1, 1, color);
            DrawLine(0, 2, 0, Surface.Properties.h - 3, 1, color);
            DrawLine(Surface.Properties.w - 1, 2, Surface.Properties.w - 1, Surface.Properties.h - 3, 1, color);
            SetPixel(1, 1, color);
            SetPixel(1, Surface.Properties.h - 2, color);
            SetPixel(Surface.Properties.w - 2, 1, color);
            SetPixel(Surface.Properties.w - 2, Surface.Properties.h - 2, color);
        }

        public void FillPolygon(int x, int y, Color fillColor)
        {
            var callList = new[] { new { X = x, Y = y } }.ToList();
            Color firstPixelColor = GetPixel(x, y);
            if (firstPixelColor.ToUint() == fillColor.ToUint())
                return;
            SetPixel(x, y, fillColor);
            while (callList.Count > 0)
            {
                var callElement = callList.First();
                if (callElement.X > 0 && GetPixel(callElement.X - 1, callElement.Y).ToUint() == firstPixelColor.ToUint())
                {
                    callList.Add(new { X = callElement.X - 1, Y = callElement.Y });
                    SetPixel(callElement.X - 1, callElement.Y, fillColor);
                }
                if (callElement.X < Surface.Properties.w - 1 && GetPixel(callElement.X + 1, callElement.Y).ToUint() == firstPixelColor.ToUint())
                { 
                    callList.Add(new { X = callElement.X + 1, Y = callElement.Y });
                    SetPixel(callElement.X + 1, callElement.Y, fillColor);
                }
                if (callElement.Y > 0 && GetPixel(callElement.X, callElement.Y - 1).ToUint() == firstPixelColor.ToUint())
                { 
                    callList.Add(new { X = callElement.X, Y = callElement.Y - 1 });
                    SetPixel(callElement.X, callElement.Y - 1, fillColor);
                }
                if (callElement.Y < Surface.Properties.h - 1 && GetPixel(callElement.X, callElement.Y + 1).ToUint() == firstPixelColor.ToUint())
                { 
                    callList.Add(new { X = callElement.X, Y = callElement.Y + 1 });
                    SetPixel(callElement.X, callElement.Y + 1, fillColor);
                }
                callList.Remove(callElement);
            }
        }

        public void DrawRectangle(Rectangle rectangle, int size, Color color)
        {
            DrawLine(rectangle.x, rectangle.y, rectangle.x + rectangle.w - 1, rectangle.y, 1, color);
            DrawLine(rectangle.x, rectangle.y + rectangle.h - 1, rectangle.x + rectangle.w - 1, rectangle.y + rectangle.h - 1, 1, color);
            DrawLine(rectangle.x, rectangle.y, rectangle.x, rectangle.y + rectangle.h - 1, 1, color);
            DrawLine(rectangle.x + rectangle.w - 1, rectangle.y, rectangle.x + rectangle.w - 1, rectangle.y + rectangle.h - 1, 1, color);
        }
    }
}
