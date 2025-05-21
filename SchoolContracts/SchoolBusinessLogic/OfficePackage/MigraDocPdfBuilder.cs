using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;
using SchoolBuisnessLogic.OfficePackage;
using Document = MigraDoc.DocumentObjectModel.Document;

namespace PipingHotBusinessLogic.OfficePackage;

internal class MigraDocPdfBuilder : BasePdfBuilder
{
    private readonly Document _document;

    public MigraDocPdfBuilder()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _document = new Document();
        DefineStyles();
    }

    public override BasePdfBuilder AddHeader(string header)
    {
        _document.AddSection().AddParagraph(header, "NormalBold");
        return this;
    }

    public override BasePdfBuilder AddParagraph(string text)
    {
        _document.LastSection.AddParagraph(text, "Normal");
        return this;
    }

    public override BasePdfBuilder AddPieChart(string title, List<(string Caption, double Value)> data)
    {
        if (data == null || data.Count == 0)
        {
            return this;
        }

        var chart = new Chart(ChartType.Pie2D);
        var series = chart.SeriesCollection.AddSeries();
        series.Add(data.Select(x => x.Value).ToArray());

        var xseries = chart.XValues.AddXSeries();
        xseries.Add(data.Select(x => x.Caption).ToArray());

        chart.DataLabel.Type = DataLabelType.Percent;
        chart.DataLabel.Position = DataLabelPosition.OutsideEnd;

        chart.Width = Unit.FromCentimeter(16);
        chart.Height = Unit.FromCentimeter(12);

        chart.TopArea.AddParagraph(title);

        chart.XAxis.MajorTickMark = TickMarkType.Outside;

        chart.YAxis.MajorTickMark = TickMarkType.Outside;
        chart.YAxis.HasMajorGridlines = true;

        chart.PlotArea.LineFormat.Width = 1;
        chart.PlotArea.LineFormat.Visible = true;

        chart.TopArea.AddLegend();

        _document.LastSection.Add(chart);

        return this;
    }

    public override Stream Build()
    {
        var stream = new MemoryStream();
        var renderer = new PdfDocumentRenderer(true)
        {
            Document = _document
        };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(stream);
        return stream;
    }

    public override BasePdfBuilder AddTable(string[] headers, List<string[]> rows)
    {
        var table = _document.LastSection.AddTable();
        table.Borders.Width = 0.25;

        // Add columns
        foreach (var _ in headers)
        {
            table.AddColumn(Unit.FromCentimeter(3));
        }

        // Add header row
        var headerRow = table.AddRow();
        headerRow.HeadingFormat = true;
        headerRow.Format.Alignment = ParagraphAlignment.Center;
        headerRow.Format.Font.Bold = true;

        for (int i = 0; i < headers.Length; i++)
        {
            headerRow.Cells[i].AddParagraph(headers[i]);
        }

        // Add data rows
        foreach (var row in rows)
        {
            var dataRow = table.AddRow();
            for (int i = 0; i < row.Length; i++)
            {
                dataRow.Cells[i].AddParagraph(row[i]);
            }
        }

        return this;
    }

    private void DefineStyles()
    {
        var style = _document.Styles.AddStyle("NormalBold", "Normal");
        style.Font.Bold = true;
    }
}
