   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CatGameCatGame  5      7 clr-namespace:CatGame.ViewModelsCatGame.ViewModels  5      -clr-namespace:CatGame.ViewsCatGame.Views  5      1clr-namespace:CatGame.HelpersCatGame.Helpers  5      5clr-namespace:CatGame.BehaviorsCatGame.Behaviors  5          CatGame.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:CatGame     5      7
viewModels clr-namespace:CatGame.ViewModels     5      -viewsclr-namespace:CatGame.Views     5      1helpersclr-namespace:CatGame.Helpers     5      5	behaviorsclr-namespace:CatGame.Behaviors     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5	        �� 	Resources  5         MinecraftFont-   'CatGame.Helpers.PurchaseStatusConverter  PurchaseStatusConverter*   $CatGame.Helpers.InverseBoolConverter  InverseBoolConverter2   ,CatGame.Helpers.VisibilityToOpacityConverter   VisibilityToOpacityConverter)   #CatGame.Helpers.ImageScaleConverter  ImageScaleConverter0   *CatGame.Helpers.ColorIndexToBrushConverter  ColorIndexToBrushConverter(   "CatGame.Helpers.DirectionConverter  DirectionConverter%   CatGame.Helpers.CenterConverter  CenterConverter)   #CatGame.Helpers.BoolToBlurConverter  BoolToBlurConverter'	   !CatGame.Helpers.HalfSizeConverter 	 HalfSizeConverter)
   #CatGame.Helpers.ArrowAngleConverter 
 ArrowAngleConverter4   .CatGame.Helpers.MoveIndicatorPositionConverter " MoveIndicatorPositionConverter4   .CatGame.Helpers.ColorForMoveIndicatorConverter " ColorForMoveIndicatorConverter5   /CatGame.Helpers.MultipleBooleansToBlurConverter # MultipleBooleansToBlurConverter �� 
TargetType �� Property �� Value  TabButtonStyle �� BasedOn-   'CatGame.Behaviors.SoundBehaviorAttacher  
HeaderText/   )CatGame.Helpers.BoolToVisibilityConverter  BoolToVisibility0  *System.Windows.Media.Animation.ElasticEase  ElasticEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode   Oscillations   Springiness-  'System.Windows.Media.Animation.SineEase  SineEaseOut  SineEaseInOut2  ,System.Windows.Media.Animation.QuadraticEase  QuadraticEaseOut.  (System.Windows.Media.Animation.CubicEase  CubicEaseInOut  ElasticEaseOut  
SineEaseIn-  'System.Windows.Media.Animation.BackEase  BackEaseOut   	Amplitude/  )System.Windows.Media.Animation.BounceEase  BounceEaseSoft	   Bounces
   
Bounciness  BounceEaseOut/  )System.Windows.Media.Animation.CircleEase  CircleEaseOut  
BackEaseIn  CubicEaseIn  CubicEaseOut  PauseMenuStyle!   CatGame.Views.PauseMenuView   CatStyle ! SkinImageStyle " MainButtonStyle # InvisibleButtonStyle �� 
TargetType �� Triggers P� Property P� Value $ ImageButtonStyle
 � Tag % 
FocusStyle �� DataType*   $CatGame.ViewModels.MainMenuViewModel    CatGame.Views.MainMenuView0   *CatGame.ViewModels.MainGameScreenViewModel&    CatGame.Views.MainGameScreenView&    CatGame.ViewModels.ShopViewModel   CatGame.Views.ShopView+   %CatGame.ViewModels.MiniGame1ViewModel!    CatGame.Views.MiniGame1View/!   )CatGame.ViewModels.MiniGamesMenuViewModel%"   CatGame.Views.MiniGamesMenuView+#   %CatGame.ViewModels.PauseMenuViewModel*$   $CatGame.ViewModels.GameOverViewModel %   CatGame.Views.GameOverView+&   %CatGame.ViewModels.MiniGame2ViewModel!'   CatGame.Views.MiniGame2View,(   &CatGame.ViewModels.CatProfileViewModel")   CatGame.Views.CatProfileView��%t  &	        &	 U     &	 h     &	 {     &	 �     &	 �     &	 �     &	 �     &	 �     &		 �     &	
      &	     &	 &    &	 9    '�� L    7   &	 �    '�� �    7��7   &	 =    7��&	 �    &	 �    &	     &	 5    &	 \    &	 }    &	 �    &	 �    &	 �    &	     &	 Y    &	 �    &	 �    &	 �    &	 "    &	 G    &	  �    &	! i    &	" �    7��&	# �	    7��&	$ �    &	% �    (�� )    5�      *, +)(�� [    5�      *, +)(�� �    5�      *, +)(�� �    5�      *, +)(�� �    5�      *,! +)(�� #    5�      *,# +)(�� U    5�      *,$ +)(�� �    5�      *,& +)(�� �    5�      *,( +)*�5   
   A=pack://application:,,,/CatGame;component/Views/#Minecraft Rus)�6j     5   
   6*     5   
   6'     5   
   6/     5   
   6&     5   
   6-     5   
   6%     5   
   6"     5   
   6&   	  5   
   6$   
  5   
   6&     5   
   61     5   
   61     5   
   62   �� 5   
   " ��6   .�5      ��  � ��8   6+   �� 5       � �� �	#FF2D2D2D6+   5!      �� 5"   
   " ��6'   .�5#      ��  � ��$ 10q�6'   �� 5$       � ��$ 20,10q�6(   �� 5%       � ��$ 20(�6)   �� 5&       � ��$ 0q�60   �� 5'       � �� �	#FFE9A5B56+   5(      �� 5*   
   " ��6   8   .�5+      ��  �@ EnableSound$
 True��6L   �� 5,       � ��8  6+   �� 5-       � ��$ 24(�6)   5.      �� 50   
   " ��6#   8   .�51      ��  � ��$ 72(�6)   �� 52       � �� �	#FFF5649E6+   53        54   
   6,     55   
   $ EaseOut=�56      $ 1��57      $ 3Q�58        5:   
   $ EaseOut=�5;        5=   
   $ 	EaseInOut=�5>        5?   
   $ EaseOut=�61     5@   
   $ 	EaseInOut=�6+     5A   
   $ EaseOut=�6-   $ 1��6B     5B   
   $ EaseIn=�6&     5C   
   $ EaseOut=�6'   $	 0.5Q�6<     5D   
   $	 2��6,   $
 1Q�68   $ EaseOut=�6G     5E   
   $ EaseOut=�5F      $	 2��5G      $	
 1.5Q�5H        5J   
   $ EaseOut=�5K        5M   
   $ EaseIn=�5N      $	 0.3Q�5O        5Q   
   $ EaseIn=�5R        5T   
   $ EaseOut=�5U      �� 5X   
   " ��6'   .�5Y      ��  � �� 5Z      �� 5\      .R�5]        5^      5`      5a      �� 5b   
   " ��6   .�5c      ��  � ��$  /Views/кoтправо.png��6'   �� 5d       � ��$	 200��6&   �� 5e       � ��$	 200��6'   5f      �� 5g   
   " ��6'   .�5h      ��  � ��$	 200��6&   �� 5i       � ��$	 200��6'   �� 5j       � ��$ UniformToFill=�6(   �� 5k       �@��BitmapScalingMode$ HighQuality=�6@   �� 5l       � ��$ Center=�64   �� 5m       � ��$ Center=�62   5n      �� 5o   
   " ��6(   8   .�5p      ��  � ��$	 300��6&   �� 5q       � ��$ 80��6'   �� 5r       � ��$ 10q�6'   �� 5s       � ��$ 24(�6)   �� 5t       � �� �	#FFF9BFCD6+   �� 5u       � �� �	#FFE9A5B56,   �� 5v       � ��$ 2q�60   5w      �� 5y   
   " ��6-   8   .�5z      ��  � ��
 ���� 6+   �� 5{       � ��
 ���� 6,   �� 5|       � ��$ 0q�60   �� 5}       � ��$ 0q�6(   �� 5~       � �� 5      �� 5�      " ��6&   .R�5�      �� 
������ ._�5�      �� 5�      5�      5�       5�      P� 5�       �@J�IsMouseOver$
 True��61   . �5�      ��  �@�Cursor$
 Hand��6/   5�      5�      �� 5�   
   " ��6)   .�5�      ��  � �� 5�      �� 5�      " ��6&   .R�5�      � .+�5�      �� #��z 6$   $��Uniform=�5�      $	��180��5�      $��40��5�      5�      5�      5�      �� 5�       � ��
 ���� 6+   �� 5�       � ��$ 0q�60   �� 5�       � ��$ 0q�6(   5�      �� 5�   
   " ��6#   .�5�      ��  �@�FocusVisualStyle 61   Z� 5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�         5�      �� 5�   
   " ! 6   .R�5�      "  5�      �� 5�   
   " # 6   .R�5�        5�      �� 5�   
   " $ 6   .R�5�      %  5�      �� 5�   
   " & 6   .R�5�      '  5�      �� 5�   
   " ( 6   .R�5�      )  5�      5�      5�      