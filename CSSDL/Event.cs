using CSSDL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public enum EventType
    {
        First = 0x0,

        Quit = 0x100,
        AppTerminating = 0x101,
        AppLowMemory = 0x102,
        AppWillEnterBackground = 0x103,
        AppDidEnterBackground = 0x104,
        AppWillEnterForeground = 0x105,
        AppDidEnterForeground = 0x106,

        Window = 0x200,
        SysWM = 0x201,
        
        KeyDown = 0x300,
        KeyUp = 0x301,
        TextEditing = 0x302,
        TextInput = 0x303,
        KeymapChanged = 0x304,

        MouseMotion = 0x400,
        MouseButtonDown = 0x401,
        MouseButtonUp = 0x402,
        MouseWheel = 0x403,

        JoyAxisMotion = 0x600,
        JoyBallMotion = 0x601,
        JoyHatMotion = 0x602,
        JoyButtonDown = 0x603,
        JoyButtonUp = 0x604,
        JoyDeviceAdded = 0x605,
        JoyDeviceRemoved = 0x606,

        ControllerAxisMotion = 0x650,
        ControllerButtonDown = 0x651,
        ControllerButtonUp = 0x652,
        ControllerDeviceAdded = 0x653,
        ControllerDeviceRemoved = 0x654,
        ControllerDeviceRemapped = 0x655,

        FingerDown = 0x0700,
        FingerUp = 0x701,
        FingerMotion = 0x702,
        
        DollarGesture = 0x800,
        DollarRecord = 0x801,
        MultiGesture = 0x802,
        
        ClipboardUpdate = 0x900,
        
        DropFile = 0x1000,
        DropText = 0x1001,
        DropBegin = 0x1002,
        DropComplete = 0x1003,
        
        AudioDeviceAdded = 0x1100,
        AudioDeviceRemoved = 0x1101,
        
        RenderTargetsReset = 0x2000,
        RenderDeviceReset = 0x2001,
        
        User = 0x8000,
        
        Last = 0xFFFF
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Event
    {
        [FieldOffset(0)]
        public EventType Type;
        [FieldOffset(0)]
        public CommonEvent Common;
        [FieldOffset(0)]
        public WindowEvent Window;
        [FieldOffset(0)]
        public KeyboardEvent Keyboard;
        [FieldOffset(0)]
        public TextEditingEvent TextEditing;
        [FieldOffset(0)]
        public TextInputEvent TextInput;
        [FieldOffset(0)]
        public MouseMotionEvent MouseMotion;
        [FieldOffset(0)]
        public MouseButtonEvent MouseButton;
        [FieldOffset(0)]
        public MouseWheelEvent MouseWheel;
        [FieldOffset(0)]
        public JoyAxisEvent JoyAxis;
        [FieldOffset(0)]
        public JoyBallEvent JoyBall;
        [FieldOffset(0)]
        public JoyHatEvent JoyHat;
        [FieldOffset(0)]
        public JoyButtonEvent JoyButton;
        [FieldOffset(0)]
        public JoyDeviceEvent JoyDevice;
        [FieldOffset(0)]
        public ControllerAxisEvent ControllerAxis;
        [FieldOffset(0)]
        public ControllerButtonEvent ControllerButton;
        [FieldOffset(0)]
        public ControllerDeviceEvent ControllerDevice;
        [FieldOffset(0)]
        public AudioDeviceEvent AudioDevice;
        [FieldOffset(0)]
        public QuitEvent Quit;
        [FieldOffset(0)]
        public UserEvent User;
        [FieldOffset(0)]
        public SysWMEvent SysWM;
        [FieldOffset(0)]
        public TouchFingerEvent TouchFinger;
        [FieldOffset(0)]
        public MultiGestureEvent MultiGesture;
        [FieldOffset(0)]
        public DollarGestureEvent DollarGesture;
        [FieldOffset(0)]
        public DropEvent Drop;

        [FieldOffset(52)]
        private UInt32 padding;
    }
}
