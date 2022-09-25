﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentFactory.Krypton.Toolkit;
using System.ComponentModel;

namespace Megahard.Controls
{
	[TypeDescriptionProvider(typeof(ForwardToolboxBitmapAttribute<KryptonNumericUpDown, ToolStripNumericUpDown>))]
	public class ToolStripNumericUpDown : ToolStripControlHost<KryptonNumericUpDown>
	{
	}
}
