using CSSDL;
using Hexx.DTO.Requests;
using Hexx.Engine.Types;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Components.Panels
{
    public class LoginForm : Form
    {
        public new bool NeedRefresh
        {
            get => childrens.Any(p => p.NeedRefresh);
        }

        private Panel form;
        private Label loginLabel;
        private InputField loginField;
        private Label passwordLabel;
        private InputField passwordField;
        private Button submitButton;
        private Button cancelButton;

        public LoginForm(Rectangle rectangle, OnSubmit callbackOnSubmit, OnCancel callbackOnCancel) : base(rectangle, callbackOnSubmit, callbackOnCancel)
        {
            loginLabel = new Label((0, 0), 16, "Login");
            loginField = new InputField((0, 20, 170, 28), "Login", 30);
            passwordLabel = new Label((0, 52), 16, "Password");
            passwordField = new InputField((0, 72, 170, 28), "Password", 60, FieldType.Password);
            submitButton = new Button((0, 114, 80, 32), "Submit", "Login", Submit);
            cancelButton = new Button((90, 114, 80, 32), "Cancel", "Cancel", Cancel);
            
            Viewport = new Viewport((rectangle.x, rectangle.y));
            form = new Panel((20, 20, 170, 146));
            AddChild(form);
            form.AddChild(loginLabel);
            form.AddChild(loginField);
            form.AddChild(passwordLabel);
            form.AddChild(passwordField);
            form.AddChild(submitButton);
            form.AddChild(cancelButton);
            Viewport = new Viewport((rectangle.x, rectangle.y, form.Viewport.Properties.w + 40, form.Viewport.Properties.h + 40));
            //Viewport.Surface
            //Viewport.Fill(new Engine.Types.Color(0xFFFFFF7Fu));
        }

        public override void Refresh()
        {
            //if (!IsVisible)
            //    return;
            if (IsVisible)
            {
                DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
                dp.DrawRoundedBorder(new CSSDL.Color() { r = 255, g = 255, b = 255, a = 127 });
                dp.FillPolygon(10, 10, new CSSDL.Color() { r = 255, g = 255, b = 255, a = 127 });
                //Viewport.Fill(new Engine.Types.Color(0xFFFFFF7Fu));
            }
            if (form.NeedRefresh)
                form.Refresh();
            base.Refresh();
            //Viewport.Draw(form);
        }

        protected override void Submit()
        {
            if (childrens.OfType<InputField>().Any(p => string.IsNullOrEmpty(p.Value.ToString())))
                throw new ArgumentException("Fill all fields and try again.");
            LoginRequest request = GenerateLoginRequest();
            data = request;
            base.Submit();
        }

        private LoginRequest GenerateLoginRequest()
        {
            UTF8Encoding encoding = new UTF8Encoding();
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.Login = (string)loginField.Value;
            loginRequest.Password = Convert.ToBase64String(new SHA512Managed().ComputeHash(new UTF8Encoding().GetBytes((string)passwordField.Value)));
            return loginRequest;
        }
    }
}
