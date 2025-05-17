using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBuisnessLogic.OfficePackage;

public abstract class BasePdfBuilder
{
    public abstract BasePdfBuilder AddHeader(string header);
    public abstract BasePdfBuilder AddParagraph(string text);
    public abstract BasePdfBuilder AddPieChart(string title, List<(string Caption, double Value)> data);
    public abstract Stream Build();
}

