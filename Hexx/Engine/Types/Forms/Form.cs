using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public delegate void OnSubmit(object data);
    public delegate void OnCancel();

    public class Form : Panel
    {
        protected object data;
        private OnSubmit onSubmit;
        private OnCancel onCancel;
        
        public Form(Rectangle rectangle, OnSubmit callbackOnSubmit, OnCancel callbackOnCancel) : base(rectangle)
        {
            onSubmit = callbackOnSubmit;
            onCancel = callbackOnCancel;
            NeedRefresh = true;
        }

        public bool AddControl(FormControl control)
        {
            AddChild(control);
            if (childrens.OfType<FormControl>().Any(p => p.Name == control.Name))
                return false;
            childrens.Add(control);
            return true;
        }

        public bool RemoveControl(FormControl control)
        {
            if (!childrens.Contains(control))
                return false;
            childrens.Remove(control);
            return true;
        }

        public bool RemoveControl(string name)
        {
            FormControl control = childrens.OfType<FormControl>().Where(p => p.Name == name).FirstOrDefault();
            if (control == null)
                return false;
            childrens.Remove(control);
            return true;
        }

        protected virtual void Submit()
        {
            //string data = string.Join(";", childrens.OfType<FormControl>().Select(p => p.Name + "=" + p.Value));
            //string data2 = string.Join(";", childrens.OfType<Panel>().SelectMany(p => p.GetActors()).OfType<FormControl>().Select(p => p.Name + "=" + p.Value));
            onSubmit?.Invoke(data);
        }

        protected virtual void Cancel()
        {
            onCancel?.Invoke();
        }
    }
}
