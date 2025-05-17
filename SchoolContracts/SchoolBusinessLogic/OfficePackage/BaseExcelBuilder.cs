using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBuisnessLogic.OfficePackage;

public abstract class BaseExcelBuilder
{
    public abstract BaseExcelBuilder AddHeader(string header, int startIndex, int count);
    public abstract BaseExcelBuilder AddParagraph(string text, int columnIndex);
    public abstract BaseExcelBuilder AddTable(int[] columnsWidths, List<string[]> data);
    public abstract Stream Build();
}
