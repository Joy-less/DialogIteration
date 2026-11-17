# DialogIteration

[![NuGet](https://img.shields.io/nuget/v/DialogIteration.svg)](https://www.nuget.org/packages/DialogIteration)

A helper library for iterating dialog character-by-character.

I decided to make this library for my own projects, because dialog character iteration
is extremely difficult to get right.

This library is designed with both English and Japanese in mind, as well as various other
languages. Some languages have unique quirks that may make this library suboptimal.

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
            NextFrameAsync: () => GDTask.NextFrame(GDTaskGlobalCancellation.GetToken()).ContinueWith(GetProcessDeltaTime).AsValueTask(),
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