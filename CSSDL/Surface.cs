using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public class Surface : BaseComponent
    {
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_CreateRGBSurface(UInt32 flags, int w, int h, int depth, UInt32 rMask, UInt32 gMask, UInt32 bMask, UInt32 aMask);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_FillRect(IntPtr destSurface, IntPtr rectangle, UInt32 color);
        
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_GetClipRect(IntPtr surface, IntPtr rectangle);

        [DllImport("SDL2_image.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr IMG_Load(string file);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_FreeSurface(IntPtr surface);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_UpperBlit(IntPtr sourceSurface, IntPtr sourceRect, IntPtr destSurface, IntPtr destRect);
        
        public Rectangle Properties { get; private set; }
        
        public Surface(Surface surface, int x, int y) : base(surface.handle)
        {
            if (surface.handle == IntPtr.Zero)
                throw new NullReferenceException("Surface passed in argument is not set.");
            Properties = new Rectangle()
            {
                x = x,
                y = y,
                w = surface.Properties.w,
                h = surface.Properties.h
            };
        }

        public Surface(Rectangle properties) : base(IntPtr.Zero)
        {
            if (properties.w < 0 || properties.h < 0)
                throw new Exception("Dimensions cannot be less 0.");
            Properties = properties;
            handle = SDL_CreateRGBSurface(0, Properties.w, Properties.h, 32, 0x000000FF, 0x0000FF00, 0x00FF0000, 0xFF000000);
        }

        public Surface(IntPtr handleToSurface, int x, int y) : base(handleToSurface)
        {
            if (handleToSurface == IntPtr.Zero)
                throw new NullReferenceException("Pointer passed in argument is null.");
            Properties = new Rectangle()
            {
                x = x,
                y = y
            };
            SetDimensionsFromHandle();
        }

        private void SetDimensionsFromHandle()
        {
            Properties = (Properties.x, Properties.y, Marshal.ReadInt32(handle, 8), Marshal.ReadInt32(handle, 12));
        }

        public static Surface LoadSprite(string path, Rectangle? position = null)
        {
            IntPtr loadedSurface = IMG_Load(path);
            if (loadedSurface == IntPtr.Zero)
                throw new NullReferenceException("Cannot load file " + path + ".");
            if (position.HasValue)
                return new Surface(loadedSurface, position.Value.x, position.Value.y);
            return new Surface(loadedSurface, 0, 0);
        }

        public void Draw(Surface objectToDraw, Rectangle? from = null, Rectangle? to = null)
        {
            IntPtr sourceRectangle = IntPtr.Zero;
            IntPtr destinationRectangle = IntPtr.Zero;
            if (from.HasValue)
            {
                sourceRectangle = Marshal.AllocHGlobal(Marshal.SizeOf(from));
                Marshal.StructureToPtr(from, sourceRectangle, false);
            }
            if (to.HasValue)
            {
                destinationRectangle = Marshal.AllocHGlobal(Marshal.SizeOf(to));
                Marshal.StructureToPtr(to, destinationRectangle, false);
            }
            IntPtr objectToDrawProperties = Marshal.AllocHGlobal(Marshal.SizeOf(objectToDraw.Properties));
            Marshal.StructureToPtr(objectToDraw.Properties, objectToDrawProperties, false);
            SDL_UpperBlit(objectToDraw.handle, sourceRectangle, handle, to.HasValue ? destinationRectangle : objectToDrawProperties);
            Marshal.FreeHGlobal(sourceRectangle);
            Marshal.FreeHGlobal(destinationRectangle);
            Marshal.FreeHGlobal(objectToDrawProperties);
        }

        public void MoveTo(int newX, int newY)
        {
            Properties = (newX, newY, Properties.w, Properties.h);
        }

        public void Clear()
        {
            CloseHandle();
            handle = SDL_CreateRGBSurface(0, Properties.w, Properties.h, 32, 0x000000FF, 0x0000FF00, 0x00FF0000, 0xFF000000);
        }

        public void Fill(Color color)
        {
            SDL_FillRect(handle, IntPtr.Zero, color.ToUint());
        }

        public override void CloseHandle()
        {
            SDL_FreeSurface(handle);
        }
    }
}
