using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TFlex;
using TFlex.Command;
using TFlex.Configuration;
using TFlex.Model;
using TFlex.Model.Model2D;
using TFlex.Model.Model3D;
using TFlex.Model.Model3D.Geometry;
using TFlex.Drawing;


namespace CircleByEdge
{
    internal abstract class CreateSplineCommandGeneral : PluginCommand
    {
        private bool _canSelect = false;
        /// <summary>
        /// Конструктор. Регистрируются обработчики событий
        /// </summary>
        /// <param name="App"></param>
        public CreateSplineCommandGeneral(Plugin App) : base(App)
        {
            //Нажата кнопка на клавиатуре или в автоменю
            this.KeyPressed += new TFlex.Command.KeyEventHandler(Command_KeyPressed);

            //Перемещение мыши
            this.ShowCursor += new TFlex.Command.MouseEventHandler(Command_ShowCursor);

            //Выход из команды - вызывается после Terminate
            this.Exit += new ExitEventHandler(Command_Exit);

            //Выбор объекта
            this.Select += new SelectEventHandler(CreateCommand_Select);
        }
        internal CreateSplineCommand CircleSpline { get; set; }
        [XmlElement]
        public double R1 { get; set; }
        [XmlElement]
        public double R2 { get; set; }
        [XmlIgnore]
        internal ReferenceHolder<Variable> VarR2;
        [XmlIgnore]
        internal ReferenceHolder<Variable> VarR1;

        /// <summary>
        /// Событие изменения состояния
        /// </summary>
        public event EventHandler InputStateChanged;
        private InputState m_state;

        public InputState State
        {
            get
            {
                return m_state;
            }
            set
            {
                TFlex.Application.ActiveMainWindow.StatusBar.Command = "Создание объекта";
                if (m_state != value)
                {
                    m_state = value;
                    switch (m_state)
                    {

                        case InputState.modeR1:
                            TFlex.Application.ActiveMainWindow.StatusBar.Prompt = "Задайте первый радиус звезды";
                            break;

                        case InputState.modeR2:
                            TFlex.Application.ActiveMainWindow.StatusBar.Prompt = "Задайте второй радиус звезды";
                            break;

                        default:
                            TFlex.Application.ActiveMainWindow.StatusBar.Prompt = string.Empty;
                            break;
                    }
                    InputStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// храним документ, для которого вызвана команда
        /// </summary>
        protected Document _document;
        /// <summary>
        /// Создаваемый объект 
        /// </summary>
        internal CreateSplineCommandGeneral Circle {  get; set; }
        public enum InputState
        {
            modePoint,   //Ничего не выбрано
            modeR1,      //Выбран центр (выбирается первый радиус)
            modeR2,      //Выбран центр и радиус (выбирается второй радиус)
            modeWait,    //Выбраны центр и оба радиуса, ожидаем подтверждения кнопкой "OK" или отката по нажатию правой кнопки мыши или кнопки "Cancel"
        };

        /// <summary>
        /// Создаваемый объект звезды
        /// </summary>


        /// <summary>
        /// Для внутреннего использования.
        /// </summary>
        /// <remarks>
        /// Обработчик события выхода из команды, вызывается после Terminate.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Command_Exit(object sender, ExitEventArgs e) { }

        /// <summary>
        /// Для внутреннего использования.
        /// </summary>
        /// <remarks>
        /// Обработчик нажатия кнопки клавиатуры или мыши.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Command_KeyPressed(object sender, TFlex.Command.KeyEventArgs e)
        {

          _document.BeginChanges("Создание фигуры");//Открытие блока изменений документа

            FreeNode n1 = new FreeNode(_document, 0, 0);

            CircleConstruction circle = new CircleConstruction(_document);
            circle.SetCenterAndRadius(n1, 50);

            Area ar = new Area(_document);
            TFlex.Model.Model2D.Contour cn = ar.AppendContour();

            ConstructionContourSegment seg1 = new ConstructionContourSegment(cn);
            seg1.Construction = circle;

            StandardWorkplane swp = new StandardWorkplane(_document, StandardWorkplane.StandardType.Top);
            AreaProfile pr3D = new AreaProfile(_document);
            pr3D.Area = ar;
            pr3D.WorkSurface = swp;

            ThickenExtrusion ex = new ThickenExtrusion(_document);

            ex.Thickness1 = 10;
            ex.LengthType = ThickenExtrusion.LengthValue.AutoValue; //Данный параметр должен быть выставлен! 
            ex.ForwardLength = 100;

            ex.Profile.Add(pr3D.Geometry.SheetContour);
        

            _document.EndChanges();//Закрытие блока изменений документа



            
        }
        
               //MessageBox.Show(found is );
                    


           
   
        

        /// <summary>
        /// Для внутреннего использования.
        /// </summary>
        /// <remarks>
        /// Метод рисует динамический курсор звезды - рассчитывает её параметры и делегирует отрисовку в StarObject.Draw(graphics)
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Command_ShowCursor(object sender, TFlex.Command.MouseEventArgs e) { }

        /// <summary>
        /// Обработчик события выделения объекта - нужен для привязки звезды к узлу
        /// </summary>
        /// <remarks>
        /// Привязка к объектам также может задаваться с помощью настройки панели селектора 
        /// </remarks>
        private void CreateCommand_Select(object sender, SelectEventArgs e) { }

        public void SetCanSelecting(bool val) {
            this._canSelect = val;
        }

        public bool GetCanSelecting()
        {
            return this._canSelect;
        }
        protected void CreatePropertiesWindow()
        {
            var propertiesWindow = new CirclePropertiesWindow(this, _document);

            //Тип заголовка в окне свойств
            propertiesWindow.PropertiesHeaderType = PropertiesWindow.HeaderType.OkPreviewCancel;

            propertiesWindow.EnableHeaderButton(PropertiesWindowHeaderButton.OK, false);
            propertiesWindow.EnableHeaderButton(PropertiesWindowHeaderButton.Preview, false);

            this.PropertiesWindow = propertiesWindow;

            //обработчик нажатий на клавиши проставляется в классах-наследниках
        }
       
        

    }
}
