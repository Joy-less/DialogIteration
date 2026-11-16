# DialogIteration

A helper library for iterating dialog character-by-character.

I decided to make this library for my own projects, because dialog character iteration
is extremely difficult to get right.

## Usage

Example for Godot with GDTask and GDTaskGlobalCancellation:

```cs
using DialogIteration;

public partial class MyExampleNode : Node {
    public DialogIterator DialogIterator { get; } = new(new DialogIteratorSettings());
    public bool ContinueFlag { get; set; } = false;

    public async GDTask PromptDialogAsync(Label DialogLabel, string Text) {
        await DialogIterator.IterateAsync(
            Text: Text,
            NextFrameAsync: () => GDTask.NextFrame(GDTaskGlobalCancellation.GetToken()).ContinueWith(GetProcessDeltaTime).AsTask(),
            OnRune: (int RuneIndex, int CharIndex, Rune Rune) => {
                DialogLabel.VisibleCharacters = RuneIndex + 1;
            },
            OnSyllable: (int RuneIndex, int CharIndex, float SyllablePitch) => {
            },
            ShouldSkip: () => ContinueFlag
        );
    }
}
```