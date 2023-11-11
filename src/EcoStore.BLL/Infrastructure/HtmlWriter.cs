using System.Text;

namespace EcoStore.BLL.Infrastructure;

public class HtmlWriter : IHtmlWriter
{
    private readonly StringBuilder _htmlBuilder = new();

    public void AddHeader(string headerText)
    {
        _htmlBuilder.AppendLine($"<h1>{headerText}</h1>");
    }

    public void AddParagraph(string paragraphText)
    {
        _htmlBuilder.AppendLine($"<p>{paragraphText}</p>");
    }

    public void AddStyles(string cssCode)
    {
        _htmlBuilder.AppendLine($"<style>{cssCode}</style>");
    }

    public void AddSubHeader(string subHeaderText)
    {
        _htmlBuilder.AppendLine($"<h2>{subHeaderText}</h2>");
    }

    public void AddTableHeader(string[] headerValues)
    {
        _htmlBuilder.AppendLine("<tr>");
        foreach (var headerValue in headerValues)
        {
            _htmlBuilder.AppendLine($"<th>{headerValue}</th>");
        }
        _htmlBuilder.AppendLine("</tr>");
    }

    public void AddTableRow(string[] rowValues, string? style = null)
    {
        _htmlBuilder.Append("<tr");
        if (style is not null)
        {
            _htmlBuilder.Append($" style=\"{style}\"");
        }

        _htmlBuilder.AppendLine(">");
        foreach (var rowValue in rowValues)
        {
            _htmlBuilder.AppendLine($"<td>{rowValue}</td>");
        }
        _htmlBuilder.AppendLine("</tr>");
    }

    public void EndTable()
    {
        _htmlBuilder.AppendLine("</table>");
    }

    public byte[] GetDocument()
    {
        return Encoding.UTF8.GetBytes(_htmlBuilder.ToString());
    }

    public void StartTable()
    {
        _htmlBuilder.AppendLine("<table>");
    }

    public void Clear()
    {
        _htmlBuilder.Clear();
    }
}