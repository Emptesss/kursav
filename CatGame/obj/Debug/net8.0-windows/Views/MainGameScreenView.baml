   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:CatGame.ViewsCatGame.Views  5      &     CatGame.Views.MainGameScreenView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:CatGame.Views     5      ��5
      �� 5   
     �� ImageSource$  /Views/fonmenu.png��6    j� Stretch$ UniformToFill=�66   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��0* �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��20,20,20,20q�6;   .+�5      �� $��/Views/монета.png��6   $	��100��6/   $	��100��6;   �� 5      $��24(�5      $��10 0 0 0q�6&   $��Center=�5      ��5      �� *	Balance+ �� StringFormat$ Баланс: {0}��5      5      � 5   
   $��1��6   $��2��6   .+�5      �� $3��-/CatGame;component/Views/котправо.png��6   $��Center=�5       $	��Top=�5!      $	��650��5"      $	��650��6    $��Uniform=�5#      $��	0,140,0,0q�6&   5$      � 5'   
   $��3��6   ���	#FFE9A5B5$	��150��63   .+�5(      �� $ 
Horizontal=�6   $��Center=�5)      $��Center=�5*      �� 5,      $��-222,-646,1130,42q�5-   	      InvisibleButtonStyle#��[  5.   	   ��5,      �� *OpenMiniGamesMenuCommand+.��5/      �� $��/Views/игры.png��6    J� RenderTransformOrigin$ 0.5,0.5&�50      $��Uniform=�51       J� Opacity$ 1Q�52      z�53      V� 54      .~�55      ��  �� ScaleX$ -1Q�6-   �� 56      �� 57      T� 58      59      ��5;      �� 5<       �� 
TargetType" ��6    	 �� Triggers	 5=      P� 5>   "   
 P� Property
 �@J�IsMouseOver P� Value$
 True��6A   . �5@   &   ��  �� Property �@J�Effect �� Value 5A   *   3  -System.Windows.Media.Effects.DropShadowEffect  5B   .      
BlurRadius$ 20Q�6?      Color$ White��5C   1      Opacity$	 0.7Q�5D   1   5F   '   �� 5H   &    �@J�RenderTransformOrigin$ 0.5,0.5&�6N   �� 5I   &    � z� 5J   *   �� 5K   .   $
 1.05Q�6=    �� ScaleY$
 1.05Q�6K   5M   '   5N   #   5P      5R      5S      �� 5T      $��1447,-568,10,75q�5U   	   #��[  5V   	   ��5T      �� *NavigateToShopCommand+.��5W      �� $��/Views/шкафчик.png��6   $
��Fill=�5X      $ 0.5,0.5&�5Y      z�5Z      V� 5[      .~�5\      �� �� 5]      �� 5^      T� 5_      5`      ��5b      �� 5c      " ��6    	 5d      P� 5e   "   
 �@J�IsMouseOver$
 True��6A   . �5f   &   ��  �@J�Effect 5g   *     5h   .   $ 20Q�6?   $ White��5i   1   $	 0.7Q�5j   1   5l   '   �� 5m   &    � z� 5n   *   �� 5o   .   $
 1.05Q�6=   $
 1.05Q�6K   5q   '   5r   #   5t      5v      5w      5x      5y      5z      