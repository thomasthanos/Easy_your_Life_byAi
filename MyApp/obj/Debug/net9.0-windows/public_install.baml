   M S B A M L   `   `   `  ����@  <MyApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MyAppMyApp  5   	       MyApp.PublicInstallWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   %localclr-namespace:MyApp     5   	     � Title$  Public Installer��5   	   $	��480��5   	   $	��650��5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5	   	   
������  � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Icon$ 
hacker.ico��5   	    � 	Resources 5         AccentColor  AccentBrush �� Color  DarkBackground  DarkBackgroundBrush3  -System.Windows.Media.Effects.DropShadowEffect  
ButtonGlow   Color	   
BlurRadius
   Opacity   	Direction  ModernButton �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius �� Triggers P� Property P� Value  ModernButtonWithEffect��%N  &	        &	 !     7   &	 9     &	 Z     7  &	 r     &	 �     7  &	 S    7  ��5   
   	#FF00B4FF��6/   �� 5   
   8   6.   ��5   
   	#FF121212��62   �� 5   
   8   66     5   
   $ #00B4FF��6.   $	 20Q�6>   $
 0Q�6N   $ 0Q�6Z   �� 5   
   " ��6%   .�5      ��  � �� �#1E1E1E6+   �� 5       � ��
 �����6+   �� 5       � ��8   6,   �� 5       � ��$	 2.5q�60   �� 5       � ��$ 14(�6)   �� 5        � ��$ Segoe UI Semibold)�6+   �� 5!       �@�Cursor$
 Hand��6'   �� 5"       � �� 5#      �� 5$      " ��6&   .R�5%      �� $ border��6!   #��z��5&   !   #��z��5'   !   #��z��5(   !   $ 5��5)   !   ._�5*      �� $��Center=�6/   $��Center=�6L   5+       5,      P� 5.       �@J�IsMouseOver$
 True��6=   . �5/   "   ��  � �� �#2A2A2A6?   �� 50   "    �@J�Effect 51   &     52   *   $ #00B4FF��6;   $	 20Q�6K   $	
 0.8Q�6[   54   #   55      P� 57       � ��$
 True��6;   . �58   "   ��  � �� �#3333336?   �� 59   "    � �� �#00C8FF6@   5:      5<      5>      5?      �� 5B   
   " ��6/   .�5C      ��  � ��
 ���� 6+   �� 5D       � �� �#6060606+   �� 5E       � ��$ 30��6&   �� 5F       � ��$ 30��6'   �� 5G       �@�Cursor$
 Hand��6'   �� 5H       � �� 5I      �� 5J      " ��6&   .R�5K      �� $ border��6!   #��z��61   $ 5��6[   ._�5L      �� $��Center=�6/   $��Center=�6L   5M       5N      P� 5O       �@J�IsMouseOver$
 True��6=   . �5P   "   �� 8    � ��6?   �� 5Q   "    � �� �	#2000B4FF6?   5R      P� 5S       � ��$
 True��6;   . �5T   "   ��  � �� �	#4000B4FF6?   5U      5W      5Y      5Z      5[      .��5]      �� #��[ 6   $ 10��6?   $��10q�6Q   ._�5^   
   � .+�5`      �� ���	#FF252526$��40��6,   $	��Top=�68   $ 	10 10 0 0��6P   ._�5a      �  � ColumnDefinitions 5b      �� 5c      $��* �6+   �� 5d      $
��Auto �6+   �� 5e      $
��Auto �6+   �� 5f      $
��Auto �6+   .+�5j      �� $��Make Your Life Easier��6    
�������$��Center=�5l       $��Center=�5m       $��16(�5n       $
��Bold"�5o       $��4��5p       �� 5s      -   5        $��1��5s      #��[ 5t      $��30��5u      $��30��5v      $	��←��5w      $��0,0,5,0q�5y      �� 5|      -   5        $��2��5|      #��[ 5}      $��30��5~      $��30��5      $	��─��5�      $��0,0,5,0q�5�      �� 5�      -   5        $��3��5�      #��[ 5�      $��30��5�      $��30��5�      $	��✕��5�      $��0,0,10,0q�5�      5�      5�      � 5�      $��Center=�6   $��Center=�6.    5�      �� 5�      $
��Auto �6'   �� 5�      $
��Auto �6'    � RowDefinitions 5�      �� 5�      $
��Auto �6$   �� 5�      $
��Auto �6$   .+�5�      �� $��0��6   $��0��6-   $��10q�6:   .+�5�      �� -   5         ��Name$ PhotoshopButton��5�      $��Adobe Photoshop��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -   5        $ LightroomClassicButton��5�      $��Adobe Lightroom Classic��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -   5        $ IllustratorButton��5�      $��Adobe Illustrator��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -   5        $ Office2024Button��5�      $��Office 2024��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      5�      �� 5�      $��1��6   $��0��6-   $��10q�6:   .+�5�      �� -   5        $ PremiereProButton��5�      $��Adobe Premiere Pro��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -	   5        $ MediaEncoderButton��5�      $��Adobe Media Encoder��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -
   5        $ ClipStudioButton��5�      $��Clip Studio Pro��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      �� 5�      -   5        $ DaVinci_Resolve��5�      $��DaVinci Resolve��5�      $	��200��5�      $��40��5�      $
��0,10q�5�      #��[ 5�      5�      5�      � 5�      -   5         �Name$ DownloadProgressBar��5�      $��2��5�      $��1��5�      $��20��5�      $	��420��5�      $��10,0,0,0q�5�      $
��Left=�5�      $	��Top=�5�       � IsIndeterminate$ False��5�      #��[ 5�      ���	#FF3E3E42���	#FF6A6A6A$��1q�5�       J� Effect 5�        5�      $	 10Q�6+      ShadowDepth$ 2Q�6;   $ Black��6K   $	
 0.5Q�6Y   ��5�      �� 5�      " �6*   .R�5�      �  �Name$ TemplateRoot��6#   .+�5�   "   �� #��z��6)   #��z��5�      #��z��5�      $ 5��5�      �� 5�   "   $ 
PART_Track��6)   #��z��5�      $ 5��5�      �� 5�   "   $ PART_Indicator��6)   #��z��5�      $ 5��5�      $
��Left=�5�      5�      5�      5�      �� 5     .+�5     �� -   5        $ 
clipstudio��5     $��40��5     $��40��5     #��[ 5      �� Left$	 450��5     
 �� Top$	 140��5	     .��5     �� $(��"/MyApp;component/Icons/install.png��6    $��26��6L   $��26��6W   5     �� 5     -   5        $ davinci��5     $��40��5     $��40��5     #��[ 5     $	 450��5     $	 200��5     .��5     �� $(��"/MyApp;component/Icons/install.png��6    $��26��6L   $��26��6W   5     5     5     �� 5     -   5        $ PauseResumeButton��5     $��Pause��5     $	��100��5     $��30��5     $��
0,391,0,39q�5     #��[ 5      $��Center=�5"     5#     5$     5%     