   M S B A M L   `   `   `  ����T  PEducationalEventGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:EducationalEventGeneratorEducationalEventGenerator  5   	   *    $EducationalEventGenerator.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:EducationalEventGenerator     5   	     � Title$  Life Skills Simulator��5   	   $	��750��5	   	   $
��1100��6    � WindowStartupLocation$ CenterScreen=�5
   	   $��Segoe UI)�5   	   ���	#FFF5F5F5 � 	Resources 5      1   +EducationalEventGenerator.AcquiredConverter   AcquiredConverter1   +EducationalEventGenerator.ProgressConverter  ProgressConverter  AnimatedProgressBar �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name	 `� 	Converter
 �� Path �� RelativeSource  ResilienceTooltip  KnowledgeTooltip  AwarenessTooltip  MotivationTooltip �� Triggers P� Property P� Value  
HeaderText  SkillHeaderText 	 SkillTemplate �� Padding � RowDefinitions d� 	IsChecked �� Mode �� 	Converter��%  &	        &	      &	 &     7  &	 �    &	 \    &	 >    &	 !    '��     &	     &	 �    &		 �    7     5   
   6"     5   
   6"   �� 5   
   " �6,   .�5      ��  � ��$ 20��6'   �� 5       � �� �	#FFE0E0E06+   �� 5       � ��$ 0q�60   �� 5       � �� 5      �� 5      " �6&   .R�5      � .+�5      �� #��z��6%   #��z��5      #��z��5      $ 4��5      ._�5   "   �� $ PART_Indicator��6)   #��z��5   !   $
��Left=�5   !   $ 4��5    !   ��5!   &   `� 5"   *   8	   .1�5#   .   �� $
 Value�66    5$   .   �� *TemplatedParent+�� 5%   .   $
 Maximum�66    5&   .   �� *TemplatedParent+�� 5'   .   $
 ActualWidth�66    5(   .   �� *TemplatedParent+5)   +   5+   #   5,      5-      5.      50      51      _� 52   
   .��53      �� HFУстойчивость снижает получаемый урон.�� 54   3   XVКаждые 10 единиц устойчивости снижают урон на 5%.�� 55   ?   ��Появляется на 6 уровне и растёт при получении урона и повышении уровня.57      58      _� 5:   
   .��5;      �� TRЗнания определяют ваш опыт и компетентность.�� 5<   :   MKВлияют на доступность некоторых решений.5>      5?      _� 5A   
   .��5B      �� caОсознанность помогает принимать взвешенные решения.�� 5C   A   ?=Повышает эффективность действий.5E      5F      _� 5H   
   .��5I      �� ZXМотивация определяет вашу энергию и стремление.�� 5J   =   IGВлияет на успешность сложных действий.5L      5M      �� 5O   
   " ��6   .�5P      ��  � �� �	#FFE0E0E06+   �� 5Q       � �� �	#FF3333336+   �� 5R       � �� �	#FF0078D76,   �� 5S       � ��$ 1q�60   �� 5T       � ��$ 8q�6'   �� 5U       � ��$
 12,6q�6(   �� 5V       � ��$ 14(�6)   �� 5W       � ��$ SemiBold"�6+   �� 5X       �@�Cursor$
 Hand��6'   �� 5Y       � �� 5Z      �� 5[      " ��6&   .R�5\      �� $ border��6!   #��z��5]   !   #��z��5^   !   #��z��5_   !   $ 4��5`   !   ._�5a      �� $��Center=�6/   $��Center=�6L   5b       5c      P� 5d       �@J�IsMouseOver . . �5e   "   ��  � �� �	#FFE5F1FB6?   �� 5f   "    � �� �	#FF0078D76@   5g      P� 5h       � �� . . �5i   "   ��  � �� �	#FFCCE4F76?   5j      P� 5k       � {� .  . �5l   "   ��  � �� �	#FFF0F0F06?   �� 5m   "    � �� �	#FFA0A0A06?   �� 5n   "    � �� �	#FFC0C0C06@   5o      5q      5s      5t      �� 5v   
   " ��6#   .�5w      ��  � �� �	#FF0078D76+   �� 5x       � ��$ 18(�6)   �� 5y       � ��$
 Bold"�6+   �� 5z       � ��$ 0,0,0,10q�6'   5{      �� 5|   
   " ��6(   .�5}      ��  � �� �	#FF0078D76+   �� 5~       � ��$ 20(�6)   �� 5       � ��$
 Bold"�6+   �� 5�       � ��$ 0,0,0,15q�6'   5�      �� 5�   
   .R�5�      �� ���	#FFF0F0F0$��2q�5�      $ 6��5�      $ 12q�5�      $��6q�5�      $	��200��5�      $	��160��5�      ��5�      �� *StatusColor+._�5�      �  5�      �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $��* �6(   �� 5�      $
��Auto �6(   .+�5�      �� $
��Bold"�5�      ���	#FF333333$��14(�5�      $
��Wrap=�5�      ��5�       �� *Name+5�      �� 5�      $��1��6    ���	#FF666666$��12(�5�      $
��Wrap=�5�      $��0,4,0,0q�5�      ��5�      �� *Description+5�      �� 5�      $��2��6    ���	#FF777777$��11(�5�      $
��Wrap=�5�      $��0,8,0,0q�5�      ��5�      �� *RequirementsText+5�      �� 5�      -   5        $��3��5�      ���	#FF333333$��Center=�5�      $��0,8,0,0q�5�       5�      �� *
IsAcquired+$ TwoWay=�{�5�      �� *
CanAcquire+��5�      �� *
IsAcquired+8   5�      5�      5�      5�      5�      .��5�      �  5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   �� 5�      $
��Auto �6   �� 5�      $
��Auto �6   .+�5�   
   �� $��0��6   ���	#FF0078D7$ 15q�65   ._�5�      �  � ColumnDefinitions 5�      �� 5�      $
��Auto �6'   �� 5�      $��* �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��0��6    �� Orientation$ 
Horizontal=�6-   $��Center=�6F   .+�5�      �� $��LIFE SKILLS SIMULATOR��6    $��24(�5�      $
��Bold"�6%   
�������5�      �� 5�      
�������$��20,0,0,0q�5�   *   $��18(�5�      $��Center=�6%   ��5�       �� *PlayerStats.Level+ �� StringFormat$ Уровень: {0}��5�   %   5�      �� 5�      -   5         ��Name$ HighScoreText��5�      $��1��5�      
�������$��18(�5�      $��Center=�5�      $��Center=�5�       � ToolTip$D >Максимальный достигнутый уровень��5�      ��5�      �� *PlayerStats.HighScore+$, &Рекордный уровень: {0}��5�      �� 5�      $��2��6   $ 
Horizontal=�5�      $��Right=�5�      .+�5�      �� -   5         ��Name$ AchievementsButton��5�      $��50��5�      $��50��5�      $��0,0,10,0q�5�      $��Center=�5�      ��5�      �� 5�      " ��6.   .R�5�   "   �� $ border��6)   $ 20��69   
������ ._�5�   &   �� $��50��6,   $��50��67   $��/achivements.png��6C    J� Effect 5�   &   3  -System.Windows.Media.Effects.DropShadowEffect  5�   *      Color$ White��6;      ShadowDepth$ 0Q�6I      
BlurRadius$ 20Q�6Y      Opacity$ 0Q�6i   5�   #    5�   "   P� 5�   &    �@J�IsMouseOver . . �5�   *   ��  �� 
TargetName$ border��61    �@J�Effect 5�   .     5�   2   $ White��6C   $ 0Q�6Q   $ 20Q�6a   $	 0.5Q�6q   5�   +   5�   '   5�      5�      �� 5�      -   5        $ 
InfoButton��5�      $��50��5�      $��50��5      $��Center=�5     ��5     �� 5     " ��6.   .R�5  "   �� $ border��6)   $ 20��69   
������ ._�5  &   �� $��50��6,   $��50��67   $��
/info2.png��6C    5  &     5  *   $ White��6;   $ 0Q�6I   $ 20Q�6Y   $ 0Q�6i   5	  #    5
  "   P� 5  &    �@J�IsMouseOver . . �5  *   �� $ border��61    �@J�Effect 5  .     5  2   $ White��6C   $ 0Q�6Q   $ 20Q�6a   $	 0.5Q�6q   5  +   5  '   5     5     5     5     5     � 5  
   $��1��6   $��10q�6    5     �� 5     $��* �6#   �� 5     $	��350 �6#   .+�5"     �� $��0��6   
�������$ 6��68   ���	#FFCCCCCC$��1q�5#  -   $ 15q�6A   ._�5$     �� $
��Auto=�6   .��5%     �� .+�5&     �� -   5        $ EventCategory��5&  $   #��[ 6;   � 5(     -   5          �Name$  
TimerPanel��5(     $y�	Collapsed=�63   $��0,5,0,10q�6J   $��30��6\    5)     �� 5*  "   $��* �63   �� 5+  "   $
��Auto �63   .+�5.     � -   5        ! �Name$! TimeProgressBar��5.  *   $��20��5/  *   ���	#FFE0E0E0���	#FF0078D7$��0q�52  *   ��53  "   �� 54  &   " �66   .R�55  *   �� $ 
PART_Track��61   #��z��56  1   $��0q�57  1   $ 4��58  1   ._�59  .   �� $ PART_Indicator��65   #��z��5:  5   $��0q�5;  5   $ 4��5<  5   $
��Left=�5=  5   5>  +   5?  '   5A     �� 5C     -   5        $ 	TimerText��5C  (   $��1��5D  (   $��10,0,0,0q�5E  (   $��16(�5F  (   $��SemiBold"�5G  (   $��Center=�5H  (   5I     �� 5K     -	   5        $ 	EventText��5K  $   $
��Wrap=�67   $��16(�5L  $   ���	#FF333333$��0,0,0,20q�6I   " �� 
LineHeight$" 24��5M  $   �� 5O     -
   5        # ��Name$# OptionsPanel��5O  %   $��	0,10,0,25q�6;   �� 5Q     -   5        $ ExplanationBox��5Q  !   $y�	Collapsed=�69   ���	#FFF5F5F5���	#FF0078D7$��1q�5S  !   $ 6��65   $ 15q�6F   ._�5T     �� .+�5U  "   �� $��Объяснение:��6,   $
��Bold"�6?   ���	#FF0078D7$��0,0,0,5q�5V  C   �� 5W  "   -   5        $ ExplanationText��5W  ,   $
��Wrap=�6E   ���	#FF555555$��14(�5X  A   �� 5Y  "   -   5        $ TemporaryEffectsText��5Y  ,   $
��Wrap=�6J   ���	#FF777777$��0,10,0,0q�5Z  A   $��13(�6S   5[     5\     �� 5]     -   5        $ TimedEventResultBox��5]  !   $y�	Collapsed=�5^  	   ���	#FFF5F5F5���	#FF0078D7$��1q�5a  	   $ 6��5b  	   $ 15q�5c  	   $��0,0,0,10q�5d  	   ._�5e     �� .+�5f  "   �� $0��*Время на ответ истекло!��6,   $
��Bold"�6K   ���	#FF0078D7$��0,0,0,8q�6t   �� 5g  "   -   5        $ TimedEventResultText��5g  ,   $
��Wrap=�6J   ���	#FF555555$��14(�6u   5i     5j     5l     5m     5n     �� 5q     $��1��6   $��10,0,0,0q�6%   
�������$ 6��5r     ���	#FFCCCCCC$��1q�6>   ._�5s     �  5t     �� 5u     $��* �6(   �� 5v     $
��Auto �6(   .+�5y     �� $
��Auto=�6#   .��5z     �� $��15q�6%   .+�5{     �� $"��ХАРАКТЕРИСТИКИ��6(   #��[ 6>   $��24(�6b   �� 5}     $��0,0,0,15q�6)   .+�5~  "   �� $ 
Horizontal=�6-   $��0,0,0,5q�6F   .+�5  &   �� $��Знания:��60   ���	#FF333333$��SemiBold"�5�  0   $��18(�5�  0   # [ 5�  0   �� 5�  &   -   5        $ KnowledgeText��5�  0   ���	#FF2E7D32$��Right=�5�  0   $��5,0,0,0q�6L   �� 5�  &   -   5        $ KnowledgeChangeText��5�  0   ���	#FF2E7D32$��5,0,0,0q�5�  0   $��14(�6A   5�  #   � 5�  "   -   5        $! KnowledgeProgress��5�  .   #��[ 5�  .   ���	#FF4CAF50$��20��5�  .   $��0,5,0,0q�5�  .   5�     �� 5�     $��0,0,0,15q�6)   .+�5�  "   �� $ 
Horizontal=�6-   $��0,0,0,5q�6F   .+�5�  &   �� $��Осознанность:��60   ���	#FF333333$��SemiBold"�5�  0   $��18(�5�  0   # [ 5�  0   �� 5�  &   -   5        $ AwarenessText��5�  0   ���	#FF1565C0$��Right=�5�  0   $��5,0,0,0q�6L   �� 5�  &   -   5        $ AwarenessChangeText��5�  0   ���	#FF1565C0$��5,0,0,0q�5�  0   $��14(�6A   5�  #   � 5�  "   -   5        $! AwarenessProgress��5�  .   #��[ 5�  .   ���	#FF2196F3$��20��5�  .   $��0,5,0,0q�5�  .   5�     �� 5�     $��0,0,0,15q�6)   .+�5�  "   �� $ 
Horizontal=�6-   $��0,0,0,5q�6F   .+�5�  &   �� $��Мотивация:��60   ���	#FF333333$��SemiBold"�5�  0   $��18(�5�  0   # [ 5�  0   �� 5�  &   -   5        $ MotivationText��5�  0   ���	#FF6A1B9A$��Right=�5�  0   $��5,0,0,0q�6L   �� 5�  &   -   5        $ MotivationChangeText��5�  0   ���	#FF6A1B9A$��5,0,0,0q�5�  0   $��14(�6A   5�  #   � 5�  "   -   5        $! MotivationProgress��5�  .   #��[ 5�  .   ���	#FF9C27B0$��20��5�  .   $��0,5,0,0q�5�  .   5�     �� 5�     -   5        $# AdvancedStatsPanel��5�  )   $y�	Collapsed=�6E   $��0,0,0,15q�6\   .+�5�  "   �� $ 
Horizontal=�6-   $��0,0,0,5q�6F   .+�5�  &   �� $��Устойчивость:��60   ���	#FF333333$��SemiBold"�5�  0   # [ 5�  0   �� 5�  &   -   5        $ ResilienceText��5�  0   ���	#FF37474F$��Right=�5�  0   $��5,0,0,0q�5�  0   �� 5�  &   -   5        $ ResilienceChangeText��5�  0   ���	#FF37474F$��5,0,0,0q�5�  0   $��14(�5�  0   5�  #   � 5�  "   -   5        $! ResilienceProgress��5�  .   #��[ 5�  .   ���	#FF607D8B$��20��5�  .   $��0,5,0,0q�5�  .   �� 5�  "   $ 
Horizontal=�6-   $��0,10,0,5q�6F   .+�5�  &   �� $��Креативность:��60   ���	#FF333333$��SemiBold"�5�  0   �� 5�  &   -   5        $ CreativityText��5�  0   ���	#FF37474F$��Right=�5�  0   $��5,0,0,0q�5�  0   �� 5�  &   -   5        $ CreativityChangeText��5�  0   ���	#FF37474F$��5,0,0,0q�5�  0   $��14(�5�  0   5�  #   � 5�  "   -   5        $! CreativityProgress��5�  .   #��[ 5�  .   ���	#FF607D8B$��20��5�  .   $��0,5,0,0q�5�  .   5�     �� 5�     $��0,25,0,0q�6)   .+�5�  "   �� $$��Прогресс уровня:��6,   ���	#FF333333$��SemiBold"�5�  ,   $��0,0,0,5q�5�  ,   $��18(�5�  ,   � 5�  "   -    5        $! LevelProgress��5�  .   #��[ 5�  .   ���	#FFFB8C00$��20��5�  .   �� 5�  "   -!   5        $ 	LevelText��5�  ,   $��14(�5�  ,   ���	#FFFB8C00$��Center=�5�  ,   $��0,5,0,0q�5�  ,   5�     5�     5�     �� 5�     $��1��6   ���	#FFF5F5F5���	#FFCCCCCC$��0,1,0,0q�5�     ._�5�     �  5�     �� 5�  "   $
��Auto �60   �� 5�  "   $��* �60   .+�5�     �� $%��АКТИВНЫЕ ЭФФЕКТЫ��6(   $
��Bold"�5�  (   ���	#FF0078D7$��
10,10,10,5q�5�  (   $��18(�5�  (   �� 5      $��1��6+   $
��Auto=�5  +   $��0,0,0,10q�5  +   .��5  "   �� -"   5        $ ��Name$$ ActiveEffectsDisplay��5  /   ��5  &   �� 5  *   .R�5  .   �� $
��10,5q�65   $
 12,8q�5  5   $��1q�5  5   $ 6��5  5   ��5	  5   �� *BorderColor+��5
  5   �� *BackgroundColor+._�5  2   �� .+�5  6   ]� ��. .+�5  :   �� $
��Bold"�5  D   $��Right=�5  D   ��5  D   �� *RemainingTurns+��5  D   �� *	TextColor+5  D   �� 5  :   $
��Bold"�5  D   ��5  D   �� *Name+��5  D   �� *	TextColor+5  D   5  7   �� 5  6   ��6C   �� *	Effects+��5  :   �� 5  >   .R�5  B   �� $��0,2,0,0q�5  L   ��5  L   �� ��5  L   �� *	TextColor+5  L   5  ?   5  7   5  3   5   /   5!  +   5#  #   5$     5%     5&     5'     5(     5)     �� 5-  
   -#   5        $ SkillsPanel��5-     $��2��5.  	   
����������	#FFCCCCCC$��0,1,0,0q�51  	   $y�	Collapsed=�52  	   ._�53     �� .+�54     � $��
15,15,15,0q�6    55     �� 56     $��* �6+   �� 57     $
��Auto �6+   .+�5:     �� $%��ДОСТУПНЫЕ НАВЫКИ��6    #��[ 5;     $��0q�5<     d� 5>     -$   5        % d�Name$% SkillsPanelToggle��5>  #   $��1��5?     $��24��5@     $��24��5A     ��5C     �� 5D     " d�6.   .R�5E  "   �� 
������ ._�5F  &   F� & F�Name$& Arrow��6+   "����        � z  z � $�     	���	#FF0078D7$��2��5I  #   $��Center=�5J  #   $��Center=�5K  #   5L  #    5M  "   P� 5N  &    �@d�	IsChecked$
 TrueY�6C   . �5O  *   �� $ Arrow��61    � ��" ��     z � z     � $�  z 	5P  )   5Q  '   5S     5U     5V     � 5X     -%   5        $  SkillsContent��5X     $��
15,0,15,15q�6.   .+�5Y     �� $
��Auto=�6#   $��Disabled=�5Z     .��5[     �� -&   5        $$ 
SkillsList��5[  '   #��[	 5\     ��5]     �� 5^  "   .R�5_  &   � ' � Orientation$' 
Horizontal=�60   5`  #   5b     5c     5d     5e     5f     �� 5i  
   $��3��6   
�������$ 10q�61   ���	#FFCCCCCC$��0,1,0,0q�5j  !   ._�5k     � .+�5l     �� $ 
Horizontal=�6   $��Center=�66   .+�5m     �� -'   5        $ StartButton��5m     $��Начать игру��62   $	��160��5n     $��40��6!   $��16(�5o  /   �� 5p     -(   5        $ 
NextButton��5p     $'��!Следующее событие��61   $	��160��5q     $��40��6!   {�.  $
��15,0q�5r  @   $��16(�5s     �� 5t     -)   5        $ RestartButton��5t     $��Начать заново��64   $	��160��5u     $��40��6!   {�.  $��16(�5w     5x     �� 5{     -*   5        $ 
ExitButton��5{     $��
Выход��5|     $	��100��5}     $��40��5~     $��16(�5�     $��Right=�5�     $��Center=�5�     $��0,0,10,0q�5�     ��5�     �� 5�     " ��6    ( �� BasedOn#( [B��64   .�5�     ��  � �� �	#FFEF53506;   �� 5�      � ��
 �����6;   ) �� Triggers) 5�     P� 5�  "    �@J�IsMouseOver . . �5�  &   ��  � �� �	#FFE539356C   5�  #   P� 5�  "    � �� . . �5�  &   ��  � �� �	#FFD32F2F6C   5�  #   5�     5�     5�     5�     �� 5�  
   -+   5        * ��Name$* EndGameImage��5�     $y�	Collapsed=�5�     $��Uniform=�5�     $	��300��5�     $	��300��5�     $��	0,10,0,10q�5�     5�     5�     