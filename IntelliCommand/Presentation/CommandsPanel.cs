// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal class CommandsPanel : Panel
    {
        public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register(
            "Commands",
            typeof(IEnumerable<CommandViewModel>),
            typeof(CommandsPanel),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                (s, e) => ((CommandsPanel)s).SetCommands((IEnumerable<CommandViewModel>)e.NewValue)));

        public static readonly DependencyProperty CombinationTextBrushProperty = DependencyProperty.Register(
            "CombinationTextBrush", 
            typeof(Brush), 
            typeof(CommandsPanel),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CommandTextBrushProperty = DependencyProperty.Register(
            "CommandTextBrush", 
            typeof(Brush), 
            typeof(CommandsPanel),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ColumnSeparatorWidthProperty = DependencyProperty.Register(
            "ColumnSeparatorWidth", 
            typeof(double), 
            typeof(CommandsPanel),
            new FrameworkPropertyMetadata(7.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RowSeparatorHeightProperty = DependencyProperty.Register(
            "RowSeparatorHeight", 
            typeof(double), 
            typeof(CommandsPanel),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        private double maxCombinationTextWidth;
        private double maxCommandTextWidth;
        private double maxFontHeight;
        private int firstColumnRows = 0;

        /// <summary>
        /// The list of commands
        /// </summary>
        public IEnumerable<CommandViewModel> Commands
        {
            get { return (IEnumerable<CommandViewModel>)this.GetValue(CommandsProperty); }
            set { this.SetValue(CommandsProperty, value); }
        }

        /// <summary>
        /// Brush for key combination text.
        /// </summary>
        public Brush CombinationTextBrush
        {
            get { return (Brush)this.GetValue(CombinationTextBrushProperty); }
            set { this.SetValue(CombinationTextBrushProperty, value); }
        }
        
        /// <summary>
        /// Brush for command name text.
        /// </summary>
        public Brush CommandTextBrush
        {
            get { return (Brush)this.GetValue(CommandTextBrushProperty); }
            set { this.SetValue(CommandTextBrushProperty, value); }
        }
        
        /// <summary>
        /// Column separator width.
        /// </summary>
        public double ColumnSeparatorWidth
        {
            get { return (double)this.GetValue(ColumnSeparatorWidthProperty); }
            set { this.SetValue(ColumnSeparatorWidthProperty, value); }
        }

        /// <summary>
        /// Row separator width.
        /// </summary>
        public double RowSeparatorHeight
        {
            get { return (double)this.GetValue(RowSeparatorHeightProperty); }
            set { this.SetValue(RowSeparatorHeightProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Children.Count == 0)
            {
                return new Size(0, 0);
            }

            this.maxFontHeight = 0;

            this.maxCommandTextWidth = 0;
            this.maxCombinationTextWidth = 0;

            for (int i = 0; i < this.Children.Count; i += 2)
            {
                var elementCombination = (TextBlock)this.Children[i];
                var elementText = (TextBlock)this.Children[i + 1];

                elementCombination.Measure(availableSize);
                elementText.Measure(availableSize);

                var maxHeight = Math.Max(elementCombination.DesiredSize.Height, elementText.DesiredSize.Height);

                if (this.maxFontHeight < maxHeight)
                {
                    this.maxFontHeight = maxHeight;
                }

                if (elementCombination.DesiredSize.Width > this.maxCombinationTextWidth)
                {
                    this.maxCombinationTextWidth = elementCombination.DesiredSize.Width;
                }

                if (elementText.DesiredSize.Width > this.maxCommandTextWidth)
                {
                    this.maxCommandTextWidth = elementText.DesiredSize.Width;
                }
            }

            this.firstColumnRows = this.Children.Count / 2;
            var columnWidth = this.maxCombinationTextWidth + this.maxCommandTextWidth + (this.ColumnSeparatorWidth * 2);
            var columnHeight = (this.maxFontHeight + this.RowSeparatorHeight) * this.firstColumnRows;

            if (columnHeight > columnWidth)
            {
                this.firstColumnRows = (this.firstColumnRows / 2) + (this.firstColumnRows % 2);
                columnHeight = (this.maxFontHeight + this.RowSeparatorHeight) * this.firstColumnRows;
                columnWidth += columnWidth + this.ColumnSeparatorWidth;
            }

            return new Size(columnWidth, columnHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double currentHeight = 0;
            double leftMargin = 0;

            for (int i = 0; i < this.Children.Count; i += 2)
            {
                if (this.firstColumnRows * 2 == i)
                {
                    leftMargin = (this.ColumnSeparatorWidth * 2) + this.maxCombinationTextWidth + this.maxCommandTextWidth;
                    currentHeight = 0;
                }

                this.Children[i].Arrange(new Rect(leftMargin, currentHeight, this.maxCombinationTextWidth, this.maxFontHeight));
                this.Children[i + 1].Arrange(new Rect(leftMargin + this.ColumnSeparatorWidth + this.maxCombinationTextWidth, currentHeight, this.maxCommandTextWidth, this.maxFontHeight));

                currentHeight += this.maxFontHeight + this.RowSeparatorHeight;
            }

            return base.ArrangeOverride(finalSize);
        }

        private void SetCommands(IEnumerable<CommandViewModel> commands)
        {
            this.Children.Clear();
            if (commands != null)
            {
                foreach (var commandViewModel in commands)
                {
                    this.Children.Add(
                        new TextBlock()
                            {
                                DataContext = commandViewModel,
                                Text = commandViewModel.KeyCombination.ToString(),
                                Foreground = this.CombinationTextBrush
                            });
                    this.Children.Add(
                        new TextBlock()
                            {
                                DataContext = commandViewModel,
                                Text = commandViewModel.Name,
                                Foreground = this.CommandTextBrush
                            });
                }
            }
        }
    }
}
