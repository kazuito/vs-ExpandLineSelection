using Microsoft.VisualStudio.Text;
using System.Linq;

namespace ExpandLineSelection {
  [Command(PackageIds.MyCommand)]
  internal sealed class MyCommand : BaseCommand<MyCommand> {
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e) {
      var docView = await VS.Documents.GetActiveDocumentViewAsync();

      var selection = docView.TextView.Selection.SelectedSpans.FirstOrDefault();
      var snapshot = selection.Snapshot;

      var selStartLine = selection.Start.GetContainingLine();
      var selEndLine = selection.End.GetContainingLine();

      var isLastLine = docView.TextView.TextSnapshot.LineCount == selEndLine.LineNumber + 1;

      var startPos = selStartLine.Start.Position;
      var endPos = selEndLine.End.Position;

      if (!isLastLine) endPos += 2;

      // specify selection area
      SnapshotSpan selectSpan = new SnapshotSpan(
           new SnapshotPoint(snapshot, startPos),
           new SnapshotPoint(snapshot, endPos));

      // make selection
      docView.TextView.Selection.Select(selectSpan, false);

      // set caret position
      docView.TextView.Caret.MoveTo(new SnapshotPoint(snapshot, endPos));
    }
  }
}
