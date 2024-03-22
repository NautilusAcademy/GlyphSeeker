using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LootBox))]
public class LootBoxGUI : Editor
{
    List<Color> col = new();


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        LootBox lootBoxScr = (LootBox)target;   //Prende lo script il quale modifica


            //--Test per Color Swatch custom--
        //EditorGUIUtility.DrawColorSwatch(new Rect(20, 10,
        //                                          EditorGUIUtility.currentViewWidth - 50, 20),
        //                                 new Color(0.85f, 0f, 0f, 0.35f));

        

        //Calcola la larghezza (width) e posizione iniziale
        //dello sfondo della percentuale finale
        float widht_bg = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - EditorGUIUtility.fieldWidth - 40,
              startX_bg = EditorGUIUtility.labelWidth + 25;

        //Crea il rettangolo
        Rect bgPercRect = new Rect(startX_bg, 195,//48.5f,
                                   widht_bg, 15);
        float test_col = 40f / 255f;

        //Disegna il Rect di sfondo
        EditorGUI.DrawRect(bgPercRect,
                           new Color(test_col, test_col, test_col, 1));

        //Prende ogni percentuale
        List<float> percs = lootBoxScr.Editor_GetAllPercents();

        #region --- Costanti utili ---

        const float SEPARATOR_WIDTH = 2,
                    PERCENT_LINE_HEIGHT = 8,
                    BORDER = 2.5F;

        //---Colori per ogni barra--//
        Color heal_s_barColor = new Color(0, 1, 0, 0.45f),
              heal_m_barColor = new Color(0, 0.7f, 0, 0.45f),
              heal_l_barColor = new Color(0, 0.4f, 0, 0.45f),
              coin_barColor = new Color(0.65f, 0.65f, 0, 0.5f),
              enemy_barColor = new Color(0.5f, 0, 0, 0.75f);

        col.Add(heal_s_barColor);
        col.Add(heal_m_barColor);
        col.Add(heal_l_barColor);
        col.Add(coin_barColor);
        col.Add(enemy_barColor);

        #endregion 



        for (int i = 0; i < percs.Count; i++)
        {
            float startPosPerc_RIN = i == 0
                                       ? BORDER
                                       : ((bgPercRect.width - BORDER * 2) * percs[i-1]) + 2,
                  width_RIN = i == 0
                                ? percs[i]
                                : (percs[i]-percs[i-1]);

            /*
            if (i == 0)
            {
                EditorGUI.DrawRect(new Rect(finalPercRect.x + BORDER + (WIDHT_SEPARATOR*0.5f), finalPercRect.yMin + 3.5f,
                                            (widht_bg - BORDER * 2) * percs[i] - (WIDHT_SEPARATOR*0.5f), HEIGHT_PERCENT_LINE),
                                   Color.green * 0.65f);
            }
            if(i > 0)
            {
                EditorGUI.DrawRect(new Rect(finalPercRect.x + ((finalPercRect.width - BORDER * 2) * percs[i-1]) + 2 + (WIDHT_SEPARATOR*0.5f), finalPercRect.yMin + 3.5f,
                                            (widht_bg - BORDER * 2) * (percs[i]-percs[i-1]) - (WIDHT_SEPARATOR*0.5f), HEIGHT_PERCENT_LINE),
                                   (i % 2 == 1 ? Color.white : Color.green) * 0.45f);
            }
            //*/

            EditorGUI.DrawRect(new Rect(bgPercRect.x + startPosPerc_RIN + (SEPARATOR_WIDTH*0.5f), bgPercRect.yMin + 3.5f,
                                        (widht_bg - BORDER * 2) * width_RIN - (SEPARATOR_WIDTH*0.5f), PERCENT_LINE_HEIGHT),
                               col[i]);//(i % 2 == 1 ? Color.yellow : Color.green) * 0.45f);
        }


        for (int i = 0; i < percs.Count; i++)
        {
            float xPos_line = bgPercRect.x + BORDER + ((widht_bg - BORDER * 3) * percs[i]),
                              yPos_line = bgPercRect.y + BORDER,
                              height_line = bgPercRect.height - 5;


            //Disegna la lineetta per ogni percentuale
            EditorGUI.DrawRect(new Rect(xPos_line, yPos_line,
                                        SEPARATOR_WIDTH, height_line),
                               new Color(0.65f, 0.65f, 0.65f, 1));
        }


        /* 
         * TODO:
         * - Rendere la linea allineata con le var.
         * - Sistemare barrette colorate (sezioni che indicano le percentuali)
         * X Sistemare barrette grige (in un altro for)
         * X Color coding (sezioni percentuali)
         * - sistema le costanti sopra/all'inizio
         */



        /*
        GUILayout.Button("Test");
        GUILayout.Label("RectTest");

        GUILayout.Box("TEst2");
        GUILayout.TextArea("\t\t Testo nfds nfuids nuidnfui dnfdjfiods fnsn foidfn osjpid no");


        GUILayout.BeginArea(new Rect(20, 75, 200, 35));

        
        GUILayout.EndArea();
        //*/
    }
}
