   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:CatGame.ViewsCatGame.Views  5      &     CatGame.Views.MainGameScreenView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:CatGame.Views     5      ��5
      �� 5   
     �� ImageSource$  /Views/fonmenu.png��6    j� Stretch$ UniformToFill=�66   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��0* �6   �� 5      �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��20,20,20,20q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� $��/Views/монета.png��6   $	��100��5      $	��100��5      $��0��5       �� 5"      $��24(�5#      $��10 0 0 0q�5$      $��Center=�5%      $��1��5&      ��5"      �� *	Balance+ �� StringFormat$ Баланс: {0}��5&      �� 5(      $��2��5)      $��Right=�5*      $	��Top=�5+      $��0,0,20,0q�5,         InvisibleButtonStyle#��[  5-      ��5(      �� *ExitCommand+.��5.      �� $'��!/Views/кнопкавыход.png��6   $	��100��5/      $	��100��50      51      52      � 55   
   $��1��6   $��2��6   � 58   
   $��3��6   ���	#FFE9A5B5$	��150��63   .+�59      ��  �� Orientation$ 
Horizontal=�6   $��Center=�5:      $��Center=�5;      �� 5<      $��-222,-646,1130,42q�5=      #��[  5>      ��5<      �� *OpenMiniGamesMenuCommand+.��5?      �� $��/Views/игры.png��6    J� RenderTransformOrigin$ 0.5,0.5&�5@      $��Uniform=�5A       J� Opacity$ 1Q�5B      z�5C      V� 5D      .~�5E      ��  �� ScaleX$ -1Q�6-   �� 5F      �� 5G      T� 5H      5I      ��5K      �� 5L      	 �� 
TargetType"	 ��6    
 �� Triggers
 5M      P� 5N   "    P� Property �@J�IsMouseOver P� Value$
 True��6A   . �5O   &   ��  �� Property �@J�Effect �� Value 5P   *   3  -System.Windows.Media.Effects.DropShadowEffect  5Q   .      
BlurRadius$ 20Q�6?      Color$ White��5R   >      Opacity$	 0.7Q�5S   >   5U   '   �� 5V   &    �@J�RenderTransformOrigin$ 0.5,0.5&�6N   �� 5W   &    � z� 5X   *   �� 5Y   .   $
 1.05Q�6=    �� ScaleY$
 1.05Q�6K   5[   '   5\   #   5^      5`      5a      �� 5b      $��1447,-568,10,75q�5c      #��[  5d      ��5b      �� *NavigateToShopCommand+.��5e      �� $��/Views/шкафчик.png��6   $
��Fill=�5f      $ 0.5,0.5&�5g      $��10,0,0,0q�5h      z�5i      V� 5j      .~�5k      �� �� 5l      �� 5m      T� 5n      5o      ��5q      �� 5r      "	 ��6    
 5s      P� 5t   "    �@J�IsMouseOver$
 True��6A   . �5u   &   ��  �@J�Effect 5v   *     5w   .   $ 20Q�6?   $ White��5x   >   $	 0.7Q�5y   >   5{   '   �� 5|   &    � z� 5}   *   �� 5~   .   $
 1.05Q�6=   $
 1.05Q�6K   5�   '   5�   #   5�      5�      5�      �� 5�      $3��-/CatGame;component/Views/котправо.png��6   $��Center=�5�      $	��Top=�5�      $	��650��5�      $	��650��5�      $��Uniform=�5�      $��
0,-542,0,0q�5�      5�      5�      5�      