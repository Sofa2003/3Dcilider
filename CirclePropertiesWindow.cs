using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlex.Command;
using TFlex.Dialogs;
using TFlex.Model;


namespace CircleByEdge
{   internal class CirclePropertiesControls
    {
        public ControlsWindowForm MainGroup { get; private set; }
        public ControlsWindowForm AdditionalGroup { get; private set; }
        public NumericInputControl InputR1 { get; private set; }
        public NumericInputControl InputR2 { get; private set; }

        public CirclePropertiesControls() 
        {
            MainGroup = new ControlsWindowForm("propMain");
            MainGroup.Caption = "Основые свойства";

            InputR1 = new NumericInputControl("inputR1");
            InputR1.Label = "Радиус 1";
            InputR1.AllowVariable = true;
            MainGroup.Controls.Add(InputR1);

            InputR2 = new NumericInputControl("inputR2");
            InputR2.Label = "Радиус 2";
            InputR2.AllowVariable = true;
            MainGroup.Controls.Add(InputR2);
        }
    }
    internal class CirclePropertiesWindow: PropertiesWindow
    {
        private CreateSplineCommandGeneral _command;
        private Document _document;
        private CirclePropertiesControls _ui;

    

        public CirclePropertiesWindow(CreateSplineCommandGeneral command, Document document)
        {
            _command = command;
            _document = document;
            _command.ShowCursor += OnShowCommandCursor;
            Caption = "Свойства объекта";

            _ui = new CirclePropertiesControls();

            _ui.InputR1.ValueChanged += InputR1_OnValueChanged;
            _ui.InputR2.ValueChanged += InputR2_OnValueChanged;
           
            AppendBaseForm(_ui.MainGroup);
            AppendBaseForm(_ui.AdditionalGroup);

            ActivateControl();


        }
        private void ActivateControl()
        {
            NumericInputControl control = null;
            switch (_command.State)
            {
                case CreateSplineCommandGeneral.InputState.modeR1: control = _ui.InputR1; break;
                case CreateSplineCommandGeneral.InputState.modeR2: control = _ui.InputR2; break;
            }
            if (control != null)
                using (control.SuppressEvents())
                    control.Activate();
        }
        private void OnInputStateChanged(object sender, EventArgs e)
        {
            ActivateControl();
        }
        private void OnShowCommandCursor(object sender, MouseEventArgs e)
        {
            
            using (_ui.InputR1.SuppressEvents())
            using (_ui.InputR2.SuppressEvents())
           
            {
                
                _ui.InputR1.SetValue(_command.CircleSpline.R1, _command.CircleSpline.VarR1.Value);
                _ui.InputR2.SetValue(_command.CircleSpline.R2, _command.CircleSpline.VarR2.Value);
                
            }
        }
        private void InputR1_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.IsValid)
            {
                _command.CircleSpline.R1 = (int)e.Value;
                _command.CircleSpline.VarR1.Value = e.Variable;
                _document.Redraw();
            }
        }
        private void InputR2_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.IsValid)
            {
                _command.CircleSpline.R2 = (int)e.Value;
                _command.CircleSpline.VarR2.Value = e.Variable;
                _document.Redraw();
            }
        }
        

    }
}
