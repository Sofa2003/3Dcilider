using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlex;
using TFlex.Command;
using TFlex.Model;

namespace CircleByEdge
{
    class CreateSplineCommand : CreateSplineCommandGeneral
    {
        public CreateSplineCommand(Plugin plugin, Document document) : base(plugin) {
            _document = document;

            /// Инициализация
			this.Initialize += new InitializeEventHandler(Command_Initialize);
        }

        /// <summary>
        /// Идентификатор команды
        /// </summary>
		public override int ID { get { return (int)Commands.SplineCreate; } }

        /// <summary>
        /// Инициализация команды:
        /// создать звезду со свойствами по умолчанию
        /// обновить automenu и привязать окно свойств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Command_Initialize(object sender, InitializeEventArgs e)
        {
            Console.WriteLine("Initialize");
            base.SetCanSelecting(true);
        }
        
    }
}
