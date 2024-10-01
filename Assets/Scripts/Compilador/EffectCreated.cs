 public class EffectCreated
{



     public void DrawEffect(List_Cards targets , Game_contex  context  )
      {
         var topCard =  context.Deck.Pop() ;
          context.Hand.Add(topCard) ;
          context.Hand.Shuffle() ;
      }




     public void DamageEffect(List_Cards targets , Game_contex  context  ,int Amount)
      {
        foreach( Card target in targets)
          {
         var i = 0 ;
        while( i < Amount)
                {
           target.Power -= 1 ;
           i += 1 ;
                }
           }
      }




}
