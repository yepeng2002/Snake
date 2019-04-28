using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Windows;

namespace Snake.App.Controls
{
    /// <summary>
    /// 带行号的文本控件
    /// </summary>
    public class LineNumTextEditor : TextEditor
    {
        public bool IsChanging { get; private set; }

        #region DependencyProperty SqlText
        public string LineNumText
        {
            get { return (string)GetValue(LineNumTextProperty); }
            set { SetValue(LineNumTextProperty, value); }
        }

        public static readonly DependencyProperty LineNumTextProperty =
            DependencyProperty.Register("LineNumText", typeof(string), typeof(LineNumTextEditor), new PropertyMetadata(string.Empty, new PropertyChangedCallback(LineNumTextEditorCallback)));

        private static void LineNumTextEditorCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineNumTextEditor lineNumTextEditor)
                if (!lineNumTextEditor.IsChanging)
                    lineNumTextEditor.Text = lineNumTextEditor.LineNumText;
        }

        #endregion SqlText

        public LineNumTextEditor()
        {
            this.TextChanged += LineNumTextEditor_TextChanged;
            this.Loaded += LineNumTextEditor_Loaded;
        }

        private void LineNumTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".txt");
        }

        private void LineNumTextEditor_TextChanged(object sender, EventArgs e)
        {
            IsChanging = true;
            SetValue(LineNumTextProperty, Text);
            IsChanging = false;
        }
    }
}
