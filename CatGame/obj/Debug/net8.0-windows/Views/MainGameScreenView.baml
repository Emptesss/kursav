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
 1.05Q�6K   5�   '   5�   #   5�      5�      5�      A� 5�      -   5         A�Name$ 	CatShadow��5�      $	��200��5�   
   $��50��5�   
   $��Center=�5�   
   $	��Top=�5�   
   $��
0,-470,0,0q�5�   
   $	 0.3Q�5�   
   ��5�      
� 5�      .��5�      �  � Color$ 	#80000000��6'    � Offset$ 0Q�69   � 5�      $ 	#00000000��6'   $ 1Q�69   5�       J� Effect 5�      -  'System.Windows.Media.Effects.BlurEffect  5�         Radius$ 15Q�6!   z�5�      V� 5�      .~�5�      �� -   5        ShadowScale��5�   )   T� 5�      -   5        ShadowTranslate�� T� Y$ 0Q�5�   F   5�       � Triggers 5�      :� 5�       :� RoutedEvent$ Loaded��6#   .[�5�      �� .��5�      ��  g� RepeatBehavior$ Forever��6)   .��5�   "   W�  �� 
TargetName$ ShadowScale��5�       �� TargetProperty$ ScaleX�5�       W� From 0.85�      	 W� To 1.26$     g� Duration$  0:0:3D�5�      ! g� AutoReverse$
! True��5�        SineEaseInOut" W� EasingFunction#" [ 5�      W� 5�   "   $ 	CatShadow��5�      $ Opacity�5�       0.45�       0.26$   $  0:0:3D�5�      $
! True��5�      5�      5�      5�      5�      �� 5�      $��Center=�5�      $	��Top=�5�      $	��650��5�      $	��650��5�      $��Uniform=�5�      $��
0,-542,0,0q�5�      ��5�      �� *CurrentCatImage+5�      5�      5�      5�      