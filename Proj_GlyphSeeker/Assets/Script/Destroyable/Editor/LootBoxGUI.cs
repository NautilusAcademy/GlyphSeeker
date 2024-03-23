using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LootBox))]
public class LootBoxGUI : Editor
{
    List<Color> percBarColors = new();


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        LootBox lootBoxScr = (LootBox)target;   //Prende lo script il quale modifica


        //Costanti utili
        const float SEPARATOR_WIDTH = 2,
                    PERCENT_LINE_HEIGHT = 8,
                    BORDER = 2.5f;

        //Colori per ogni barra
        Color heal_s_barColor = new Color(0, 0.7f, 0.42f, 0.5f),
              heal_m_barColor = new Color(0, 0.5f, 0.3f, 0.5f),
              heal_l_barColor = new Color(0, 0.35f, 0.2f, 0.6f),
              coin_barColor = new Color(0.65f, 0.65f, 0, 0.5f),
              enemy_barColor = new Color(0.5f, 0, 0, 0.75f);

        percBarColors.Clear();
        percBarColors.Add(heal_s_barColor);
        percBarColors.Add(heal_m_barColor);
        percBarColors.Add(heal_l_barColor);
        percBarColors.Add(coin_barColor);
        percBarColors.Add(enemy_barColor);

        //Colori del separatore
        Color separator_barColor = new Color(0.75f, 0.75f, 0.75f, 1);



        #region Rettangolo di sfondo

        //Calcola la larghezza (width), posizione iniziale
        //e colore (grigio scuro)
        //dello sfondo della percentuale finale
        float widht_bg = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - EditorGUIUtility.fieldWidth - 40,
              startX_bg = EditorGUIUtility.labelWidth + 25,
              col_bg = 40f / 255f;


        //Crea il rettangolo
        Rect bgPercRect = new Rect(startX_bg, 195,//48.5f,
                                   widht_bg, 15);

        //Disegna il Rect di sfondo
        EditorGUI.DrawRect(bgPercRect,
                           new Color(col_bg, col_bg, col_bg, 1));

        #endregion



        #region Barre delle percentuali

        //Prende ogni percentuale
        List<float> percs = lootBoxScr.Editor_GetAllPercents();


        //Disegna ogni barra della percentuale colorata
        for (int i = 0; i < percs.Count; i++)
        {
            //Largezza della barra interna
            //(senza bordo)
            float thisPerc = percs[i],
                  prevPerc = i == 0 ? 0 : percs[i-1];
            
            //Largezza della barra interna
            //(senza bordo)
            float widht_bg_noBorder = widht_bg - BORDER * 2;

            //Posiz. X iniziale e larghezza
            //per le percentuali
            float width_perc = thisPerc - prevPerc,
                  startX_perc = i == 0
                                  ? BORDER
                                  : (widht_bg_noBorder * prevPerc + 2);


            //Disegna ogni barra della percentuale
            EditorGUI.DrawRect(new Rect(bgPercRect.x + startX_perc, bgPercRect.y + BORDER + 1,
                                        widht_bg_noBorder * width_perc, PERCENT_LINE_HEIGHT),
                               percBarColors[i]);
        }

        #endregion



        #region Separatori

        //Disegna ogni "separatore"
        //(la lineetta che corrisponde al valore max
        // della percentuale alla sua sinistra)
        for (int i = 0; i < percs.Count; i++)
        {
            float xPos_line = bgPercRect.x + BORDER + ((widht_bg - BORDER * 3) * percs[i]),
                  yPos_line = bgPercRect.y + BORDER,
                  height_line = bgPercRect.height - (BORDER*2),
                  yPos_addedLine = bgPercRect.y + bgPercRect.height + 2.5f;


            //Disegna la lineetta per ogni percentuale
            EditorGUI.DrawRect(new Rect(xPos_line, yPos_line,
                                        SEPARATOR_WIDTH, height_line),
                               separator_barColor);


            //Disegna un segnale per indicare
            //che una percentuale e' zero
            //(ovvero che una barretta si trova sopra l'altra)
            if (i > 0 && percs[i] - percs[i-1] <= 0.01f)
            {
                EditorGUI.DrawRect(new Rect(xPos_line, yPos_addedLine,
                                            SEPARATOR_WIDTH, SEPARATOR_WIDTH),
                                   separator_barColor);
            }
        }

        #endregion
    }
}
