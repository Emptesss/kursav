   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CatGame.HelpersCatGame.Helpers  5      /clr-namespace:CatGame.ModelsCatGame.Models  5      -clr-namespace:CatGame.ViewsCatGame.Views  5      7 clr-namespace:CatGame.ViewModelsCatGame.ViewModels  5	      -clr-namespace:CatGame.ViewsCatGame.Views  5
      !    CatGame.Views.MiniGame2View   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1helpersclr-namespace:CatGame.Helpers     5      /modelsclr-namespace:CatGame.Models     5      -localclr-namespace:CatGame.Views     5      7
viewModels clr-namespace:CatGame.ViewModels     5	      -viewsclr-namespace:CatGame.Views     5
      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      ���	#FFF9BFCD  � 	Resources  5      0   *CatGame.Helpers.ColorIndexToBrushConverter   ColorIndexToBrushConverter%   CatGame.Helpers.CenterConverter  CenterConverter)   #CatGame.Helpers.BoolToBlurConverter  BoolToBlurConverter/   )CatGame.Helpers.BoolToVisibilityConverter  BoolToVisibilityConverter'   !CatGame.Helpers.HalfSizeConverter  HalfSizeConverter)   #CatGame.Helpers.ArrowAngleConverter  ArrowAngleConverter4   .CatGame.Helpers.MoveIndicatorPositionConverter " MoveIndicatorPositionConverter5   /CatGame.Helpers.MultipleBooleansToBlurConverter # MultipleBooleansToBlurConverter��%�   &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �       5   
   6-     5   
   6"     5   
   6&     5   
   6,     5   
   6$     5   
   6&     5   
   61     5   
   62   5      .��5      � .+�5    
   � -   5         �Name$ GameContent��5        J� Effect 5!      -	  'System.Windows.Media.Effects.BlurEffect	  5"       	  Radius 5#      `� 5$       `� 	Converter# [ 6'   .1�5%      ��  �� Path$ IsPaused�6&   �� 5&      $ 
IsGameOver�6&   5'      5)      .+�5,      � $��20q�6   $	��Top=�6    � ColumnDefinitions 5-      �� 5.      $
��Auto �6'   �� 5/      $��* �6'   �� 50      $
��Auto �6'   .+�54      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   .+�55      �� $/��)/CatGame;component/Views/монета.png��6   $��60��56      $��60��6   �� 57      $��28(�58      $
��10,0q�59      $��Center=�5:      ��57       �� *GameData.CurrentGameBalance+5:      5;      �� 5>      $��2��6     InvisibleButtonStyle#��[ 5?      ��5>   )   �� *PauseCommand+.��5@      �� $9��3/CatGame;component/Views/кнопкапауза.png��6   $��60��5A      $��60��6'   5B      5C      �� 5F      -   5        +
   %CatGame.ViewModels.MiniGame2ViewModel 
  
FieldWidth#��Z 5F      	 
  FieldHeight#��Z	 5G      $��Center=�5H      $	��Top=�5I      $��0,70,0,0q�5J      
������ .+�5P      �� $	��320��5R      $	��320��5S      
 J� RenderTransformOrigin$
 0.5,0.5&�5V      ��5P      �� *!GameData.SelectedSkin.ImagePath+ 
  DefaultCatImage �� TargetNullValue# Z  �� Left 5T      �� *CatPosition.X+
 �� Top 5U      �� *CatPosition.Y+z�5W      �� 5X       �� ScaleX$ -1Q�6)   5Z      �� 5_      .+�5a      �� ���#FF7B8C$��3��5f      	 �� X1 5a      �� *CurrentBubblePos.X+	 �� Y1 5b      �� *CurrentBubblePos.Y+	 �� X2 5c      �� *AimDirection.X+	 �� Y2 5d      �� *AimDirection.Y+5f      F� 5i      ���#FF7B8C���#FF7B8C$��1��5k       K� ZIndex$	 999��5l      ��5m      � 5n      .��5o   "   B� .��5p   &   E�  E� IsClosed$
 True��61    E� 
StartPoint 5q   *   8� 5r   .    8� X$ 0Q�64    8� Y$ 0Q�6:   .��5t   *   �  � Points# �   �ig��ig���� �ig���� 6:   5u   '   5v   #   5w      z�5y      V� 5z      .~�5{   "   ��  �� Angle 5|   &   `� 5}   *   # [ 67   .1�5~   .   �� $ CurrentBubblePos�66   �� 5   .   $ AimDirection�66   5�   +    �� CenterX$ 0Q�5�   Y    �� CenterY$ 0Q�5�   Y   5�   #   T� 5�   "    T� X 65   �� *AimDirection.X+ T� Y 5�   #   �� *AimDirection.Y+5�      5�      5�      �� 5�      $	 999��5�      ��5�      �� *MoveIndicators+ �� UpdateSourceTrigger$ PropertyChanged=���5�      �� 5�      .R�5�      �� 5�      ��5�      �� 5�        �� 
TargetType"  ��6    .�5�      �� ! �� Property! �@��Left" �� Value" 6<   �� *.+# �� 	Converter## [ �� 5�      ! �@��Top$	" 950��6;   5�      5�      ��5�      �� 5�      .R�5�      A� $ 
  
BubbleSize#��Z$ 6&   #��Z$ 5�      
���   �$��2��5�      % J� Opacity$	% 0.7Q�5�      ��5�   "   �� 5�   &   & �� Color& 5�   *   `� 5�   .    "	 ColorForMoveIndicatorConverter# [	 6;   .1�5�   2   �� $ .�6:   �� 5�   2   $ DataContext.NextColor�6:   ' �� RelativeSource' 5�   &   �� ( �� AncestorType( ItemsControl5�   /   5�   '   5�      5�      5�      A� 5�      #��Z$ 6   #��Z$ 5�   
   
���   �$��2��5�   
   $	 999��5�   
   z�5�      T� 5�       5�      �� *CurrentBubblePos.X+## [  5�      �� *CurrentBubblePos.Y+## [ ��5�      �� 5�      $ CurrentColor�6"   ## [  5�      5�      �� 5�      ��6   �� *	Bubbles+��5�      �� 5�      .R�5�      �� 5�      ��5�      �� 5�      "  ��6    .�5�      �� ! �@��Left" 6<   �� *
Position.X+�� 5�      ! �@��Top" 6;   �� *
Position.Y+�� 5�      ! �@K�ZIndex" 6=   �� *Row+5�      ��5�      �� 5�      .R�5�      A� #��Z$ 6&   #��Z$ 5�   &   
���   �$��2��5�   &   ��5�   "   �� 5�   &   $ 
ColorIndex�6.   ## [  5�   .   5�      5�      5�      �� 5�      $
 1000��5�      $ 70��5�      ��5�      �� *ParticleEffect.Particles+��5�      �� 5�      .R�5�      �� 5�      ��5�      �� 5�      "  ��6    .�5�      �� ! �@��Left" 6<   �� *
Position.X+�� 5�      ! �@��Top" 6;   �� *
Position.Y+5�      ��5�      �� 5�      .R�5�      F� ) �� Stretch$) Uniform=�5�      $
 0.5,0.5&�5�      ��5�   #   �� *Size+��5�      �� *Size+% 5�      �� *	Opacity+��5�   "   �� 5�   &   "  F�6,   * �� Triggers* 5�   *   �� 5   .   + �� Binding+ 6:   �� *	IsShard+, �� Value, True6V   .a�5  2   �� ! � ��" 5  6   B� 5  :   .��5  >   E� $ 0.5,0&�6I   $
 True��6\   .��5  B   � / �    5 �� `�
  � `�
 @ �� 6R   5  ?   5  ;   5
  3   5  /   �� 5  .   + 6:   �� *	IsShard+	, False6V   .a�5  2   �� ! � ��" 5  6   B� 5  :   .��5  >   E� $ 0,0.5&�6I   $
 True��6\   .��5  B   �  �    �  �  � 6R   5  ?   5  ;   5  3   5  /   5  '   ��5  "   �� 5  &   & 66   �� *Color+z�5  "   V� 5  &   .~�5   *   ��  6:   �� *
Rotation+T� 5!  *    5"     �� *Size+## [  5#     �� *Size+## [ 5$  '   5&     5'     5)     5*     5,     � 5/  
   $
 1000��6   y�50     �� *
IsPaused+## [ .+�51     �� $��Center=�52     $��Center=�53     ��51     �� *CurrentView+53     54     � 57  
   $
 2000��6   y�58     �� *
IsGameOver+## [ .+�59     �� $��Center=�5:     $��Center=�5;     ��59     �� *GameOverViewModel+5;     5<     5=     5>     