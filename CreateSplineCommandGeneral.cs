using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFlex;
using TFlex.Command;
using TFlex.Configuration;
using TFlex.Model;
using TFlex.Model.Model2D;
using TFlex.Model.Model3D;
using TFlex.Model.Model3D.Geometry;


namespace CircleByEdge
{
    class CreateSplineCommandGeneral : PluginCommand
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

        /// <summary>
        /// храним документ, для которого вызвана команда
        /// </summary>
        protected Document _document;

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
        
           



            //FreeNode n1 = new FreeNode(_document, 0, 0);



            //// Замыкаем контур линиями

            //CircleConstruction circle = new CircleConstruction(_document);
            //circle.SetCenterAndRadius(n1, 8.5);
            ////CircleConstruction circle1 = new CircleConstruction(_document);
            ////circle1.SetCenterAndRadius(n1, 13);



            //// Создаем штриховку и ее контур
            //Area ar1 = new Area(_document);
            //TFlex.Model.Model2D.Contour cn = ar1.AppendContour();
            ////Area ar2 = new Area(_document);
            ////TFlex.Model.Model2D.Contour cn1 = ar2.AppendContour();
            //// Описываем контур штриховки сегментами

            //ConstructionContourSegment seg1 = new
            //ConstructionContourSegment(cn);
            //seg1.Construction = circle;

            ////ConstructionContourSegment seg2 = new
            ////ConstructionContourSegment(cn1);
            ////seg2.Construction = circle1;






            //// Создаем стандартную рабочую плоскость
            //// Top - вид спереди, Front - вид спереди, Left - вид слева и др
            //StandardWorkplane swp1 = new
            //StandardWorkplane(_document, StandardWorkplane.StandardType.Top);

            //StandardWorkplane swp2 = new
            //StandardWorkplane(_document, StandardWorkplane.StandardType.Top);


            //// Создаем 3D-профиль на основе штриховки и рабочей плоскости
            //AreaProfile ap1 = new AreaProfile(_document);
            ////ap1.Area = ar1;
            //ap1.WorkSurface = swp1;

            //AreaProfile ap2 = new AreaProfile(_document);
            ////ap2.Area = ar2;
            //ap2.WorkSurface = swp2;

            //// Создаем операцию выталкивания
            //ThickenExtrusion extr = new ThickenExtrusion(_document);

            //// Длина выталкивания для первого направления
            //extr.Thickness1 = 5;

            //// Профиль для выталкивания
            //extr.Profile.Add(ap2.Geometry.SheetContour);
            //extr.Profile.Add(ap1.Geometry.SheetContour);




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

    }
}
