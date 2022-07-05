using Hexx.Components.Panels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexx.Components.Levels
{
    public class MainMenu : Level
    {
        private MenuContainer menuContainer = new MenuContainer();
        private LoginForm loginForm;
        private RegisterForm registerForm;
        private Logo logo;

        public MainMenu(Viewport viewport) : base(viewport)
        {
            menuContainer.AddElement("play", "Play", ShowLoginForm);
            menuContainer.AddElement("register", "Register", ShowRegisterForm);
            menuContainer.AddElement("exit", "Exit", CSSDL.Core.Quit);
            menuContainer.Move(0, 40);
            AddActor(menuContainer);
            loginForm = new LoginForm(new CSSDL.Rectangle(), LoginToGame, LoginCanceled);
            loginForm.MoveTo((640 - loginForm.Viewport.Properties.w) / 2, 240);
            AddActor(loginForm);
            loginForm.IsVisible = false;
            registerForm = new RegisterForm(new CSSDL.Rectangle(), RegisterUser, RegisterCanceled);
            registerForm.MoveTo((640 - registerForm.Viewport.Properties.w) / 2, 240);
            AddActor(registerForm);
            registerForm.IsVisible = false;
            logo = new Logo(new CSSDL.Rectangle(215, 40));
            AddActor(logo);
        }

        public override void Dispose()
        {
        }

        private void ShowLoginForm()
        {
            loginForm.IsVisible = true;
            menuContainer.IsVisible = false;
        }

        private void ShowRegisterForm()
        {
            registerForm.IsVisible = true;
            menuContainer.IsVisible = false;
        }

        private void Quit()
        {
            CSSDL.Core.Quit();
            Environment.Exit(0);
        }

        private void LoginToGame(object data)
        {
            ConnectionManager connection = ConnectionManager.GetInstance();
            try
            {
                loginForm.CanInteract = false;
                if (connection.IsConnected)
                    connection.Disconnect();
                connection.Connect(ConnectionManager.DEFAULT_SERVER_IP, ConnectionManager.DEFAULT_PORT);
                IncomingMessageManager.AddIncomingMessageListener<LoginResponse>(LoginCallback);
                connection.Send(data as Request);
            }
            catch (Exception ex)
            {
                IncomingMessageManager.RemoveIncomingMessageListener<LoginResponse>(LoginCallback);
                MessageBoxService.GetInstance(this).Show(ex.Message, MessageBoxButtons.OK, MainMenuMessageBoxAction, true);
            }
        }

        private void LoginCallback(LoginResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<LoginResponse>(LoginCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.OnDeniedMessageBoxOK += MainMenuMessageBoxAction;
            processor.OnErrorMessageBoxOK += MainMenuMessageBoxAction;
            processor.Process();
            if (response.Status != ResponseStatus.OK)
            {
                loginForm.CanInteract = false;
            }
        }

        private void RegisterUser(object data)
        {
            ConnectionManager connection = ConnectionManager.GetInstance();
            try
            {
                registerForm.CanInteract = false;
                if (connection.IsConnected)
                    connection.Disconnect();
                connection.Connect(ConnectionManager.DEFAULT_SERVER_IP, ConnectionManager.DEFAULT_PORT);
                IncomingMessageManager.AddIncomingMessageListener<RegisterResponse>(RegisterCallback);
                connection.Send(data as Request);
            }
            catch (Exception ex)
            {
                IncomingMessageManager.RemoveIncomingMessageListener<RegisterResponse>(RegisterCallback);
                MessageBoxService.GetInstance(this).Show(ex.Message, MessageBoxButtons.OK, MainMenuMessageBoxAction, true);
            }
        }
        private void RegisterCallback(RegisterResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<RegisterResponse>(RegisterCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.OnDeniedMessageBoxOK += MainMenuMessageBoxAction;
            processor.OnErrorMessageBoxOK += MainMenuMessageBoxAction;
            processor.Process();
            if (response.Status != ResponseStatus.OK)
            {
                loginForm.CanInteract = false;
            }
        }

        private void LoginCanceled()
        {
            loginForm.IsVisible = false;
            menuContainer.IsVisible = true;
        }

        private void RegisterCanceled()
        {
            registerForm.IsVisible = false;
            menuContainer.IsVisible = true;
        }

        private void MainMenuMessageBoxAction(MessageBoxResult result)
        {
            loginForm.CanInteract = true;
            registerForm.CanInteract = true;
        }
    }
}
