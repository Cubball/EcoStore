namespace EcoStore.BLL.Infrastructure;

public interface IHtmlWriter
{
    void AddHeader(string headerText);

    void AddSubHeader(string subHeaderText);

    void AddParagraph(string paragraphText);

    void AddStyles(string cssCode);

    void StartTable();

    void AddTableHeader(string[] headerValues);

    void AddTableRow(string[] rowValues, string? style = null);

    void EndTable();

    byte[] GetDocument();
}