using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Megahard.ExtensionMethods;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Tasks
{
	public interface ITaskContainer : IProgressIndicator
	{
		UITask ActiveTask { get; set; }
	}
}