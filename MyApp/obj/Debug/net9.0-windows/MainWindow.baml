   M S B A M L   `   `   `  ����@  <MyApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MyAppMyApp  5   	       MyApp.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:MyApp     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��600��6)    � WindowStyle$
 None=�5	   	    � AllowsTransparency$
 True��6   
������  � WindowStartupLocation$ CenterScreen=�5
   	    � 	Resources 5         ModernButton �� 
TargetType �� Property �� Value&   System.Windows.Media.TextOptions3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius	   ShadowDepth
   Color �� 
TargetType �� CornerRadius J� SnapsToDevicePixels � UseLayoutRounding  ModernButtonWithHover �� BasedOn �� Triggers P� Property P� Value N� EnterActions	 �� To g� Duration �� TargetProperty N� ExitActions��%>  &	        &	 J    7   �� 5   
   " ��6%   .�5      ��  � ��$ 17(�6)   �� 5       � ��$ Comic Sans MS)�6+   �� 5       � ��
 �   �6+   �� 5       � ��
 �����6+   �� 5       � ��
 �   �6,   �� 5       � ��$ 2q�60   �� 5       � ��$ 5q�6(   �� 5       �@J�SnapsToDevicePixels$
 True��64   �� 5       �@�UseLayoutRounding$
 True��62   �� 5       �@ TextFormattingMode$ Ideal=�6?   �� 5       �@ TextRenderingMode$ 	ClearType=�6>   �� 5       �@��BitmapScalingMode$ HighQuality=�6@   �� 5       �@��EdgeMode$ Aliased=�67   �� 5       �@�Cursor$
 Hand��6'   �� 5       �@J�Effect 5        5      $ 10Q�6'   $	 0Q�67   $
 Black��6G   5!      �� 5"       � ��$
 Bold"�6+   �� 5#       � �� 5$      �� 5%      " ��6&   .R�5&      �� #��z��6!   #��z��5'   !   #��z��5(   !   $ 5��5)   !   $
 True��5*   !   $
 True��5+   !   ._�5,      �� $��Center=�6/   $��Center=�6L   5-      5.      50      51      �� 54   
   " ��6.   8    55      P� 56       �@J�IsMouseOver$
 True��61   . �57      ��  � ��
 �����63   �� 58       � ��
 ��  �63   �� 59       � ��
 ��  �64    5:      �� 5;      .��5<      �� .��5=   "   ��  DarkBlue61   $ 0:0:0.9D�6?   $2 ,(Border.BorderBrush).(SolidColorBrush.Color)�6R   5>      5?       5A      �� 5B      .��5C      �� .��5D   "   �� 	 Black61   $ 0:0:0.9D�6<   $2 ,(Border.BorderBrush).(SolidColorBrush.Color)�6O   5E      5F      5H      P� 5I       � ��$
 True��6/   . �5J      ��  � ��
 �����63   �� 5K       � ��
 �����63   �� 5L       � ��
 �   �64   5M      5O      5P      .��5R      � .+�5T   
   �� ���	#FF2D2D30$ 10��6(   $��10q�6:   ._�5U      � .+�5W      �� ���	#FF252526$��40��60   $	��Top=�6<   $ 	10 10 0 0��6T   ._�5X      �  � ColumnDefinitions 5Y      �� 5Z      $��* �6/   �� 5[      $
��Auto �6/   �� 5\      $
��Auto �6/   .+�5`      �� $��Make Your Life Easier��6$   
�������$��Center=�6T   $��Center=�6o   $��18(�6�   $
��Bold"�6�   $��3��6�   $��Comic Sans MS)�6�   �� 5c      -   5        $��1��5c   !   #��[ 61   $��30��6`   $��30��6k   $	��─��6w   $��0,0,5,0q�6�   
������ 
�������6�   �� 5f      -   5        $��2��5f   !   #��[ 61   $��30��6`   $��30��6k   $	��✕��6w   $��0,0,10,0q�6�   
������ 
�������6�   5g      5h      � 5k      $��Center=�6   $��Center=�62    � RowDefinitions 5l      �� 5m      $
��Auto �6(   �� 5n      $��20 �6(   �� 5o      $
��Auto �6(   �� 5p      $��20 �6(   �� 5q      $
��Auto �6(    5s      �� 5t      $
��Auto �6+   �� 5u      $��20 �6+   �� 5v      $
��Auto �6+   .+�5{      �� -   5        $��Install Apps��5{      #��[  64   $	��200��6Z   $��40��6f   $��0��6r   $��0��6    J� RenderTransformOrigin$ 0.5, 0.5&�5|   	   z�5~      �� 5       �� ScaleX$ 1Q�6-    �� ScaleY$ 1Q�68    � Triggers 5�      :� 5�       :� RoutedEvent$ Button.MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W� 	 W� To 1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleX�6W   W� 5�   *    1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleY�6W   5�   '   5�   #   5�      :� 5�      $ Button.MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleX�6T   W� 5�   *    16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleY�6T   5�   '   5�   #   5�      5�      �� 5�      $��Crack Sites��6   #��[  63   $	��200��6Y   $��40��6e   $��0��6q   $��2��6~   $ 0.5, 0.5&�5�      z�5�      �� 5�      $ 1Q�6-   $ 1Q�68    5�      :� 5�      $ Button.MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleX�6W   W� 5�   *    1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleY�6W   5�   '   5�   #   5�      :� 5�      $ Button.MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleX�6T   W� 5�   *    16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleY�6T   5�   '   5�   #   5�      5�      �� 5�      $3��-Sys Maintenance: Updates, Temp Del, PC Health��6   #��[  6U   $	��420��6{   $��40��6�   $��2��6�   $��0��6�   $��3��6�   $ 0.5, 0.5&�5�      z�5�      �� 5�      $ 1Q�6-   $ 1Q�68    5�      :� 5�      $ Button.MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleX�6W   W� 5�   *    1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleY�6W   5�   '   5�   #   5�      :� 5�      $ Button.MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleX�6T   W� 5�   *    16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleY�6T   5�   '   5�   #   5�      5�      �� 5�      $��Activate / Auto Login��6   #��[  6=   $	��200��6c   $��40��6o   $��4��6{   $��0��6�   $ 0.5, 0.5&�5�      z�5�      �� 5�      $ 1Q�6-   $ 1Q�68    5�      :� 5�      $ Button.MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleX�6W   W� 5�   *    1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleY�6W   5�   '   5�   #   5�      :� 5�      $ Button.MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleX�6T   W� 5�   *    16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleY�6T   5�   '   5�   #   5�      5�      �� 5�      $��
Crack Apps��6   #��[  62   $	��200��6X   $��40��6d   $��4��6p   $��2��6}   $ 0.5, 0.5&�5�      z�5�      �� 5�      $ 1Q�6-   $ 1Q�68    5�      :� 5�      $ Button.MouseEnter��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleX�6W   W� 5�   *    1.056:   $ 0:0:0.2D�6D   $ RenderTransform.ScaleY�6W   5�   '   5�   #   5�      :� 5�      $ Button.MouseLeave��6+   .[�5�   "   �� .��5�   &   �� .��5�   *   W�  16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleX�6T   W� 5�   *    16:   $ 0:0:0.2D�6A   $ RenderTransform.ScaleY�6T   5�   '   5�   #   5�      5�      5�      5�      5      5     5     