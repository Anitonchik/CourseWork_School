using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBuisnessLogic.OfficePackage;

public abstract class BaseWordBuilder
{
    public abstract BaseWordBuilder AddHeader(string header);
    public abstract BaseWordBuilder AddParagraph(string text);
    public abstract BaseWordBuilder AddTable(int[] widths, List<string[]> data);
    public abstract Stream Build();
}
