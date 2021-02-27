using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebugController : MonoBehaviour
{
    [SerializeField] private bool showConsole = false;
    [SerializeField] private bool showHelp = false;
    [SerializeField] private bool focus = false;
    [SerializeField] private string input;
    [SerializeField] private string lastCommand;
    public float step, size;
    public LayerMask enemyLayer;
    public Transform enemiesTransform;
    private Player player;

#region COMMANDS
    public static DebugCommand<bool> IGNORE_COMMANDS;
    public static DebugCommand<int> SET_HP;
    public static DebugCommand<int> SET_MP;
    public static DebugCommand<int> SET_SP;
    public static DebugCommand<int, int> ADJUST_TOOLTIP;
    public static DebugCommand<int, int> SET_LEFT_MENU_SIZE;
    public static DebugCommand<bool> SHOW_FPS;
    public static DebugCommand<bool> SHOW_INPUTS;
    public static DebugCommand<string,int> ADD_ENEMY;
    public static DebugCommand HELP;

    [SerializeField] private List<DebugCommandBase> commandList;
#endregion

    // Start is called before the first frame update
    void Awake()
    {
        player = Player.Instance;
        enemyLayer = enemyLayer = 1 << 9;
        enemiesTransform = Resources.FindObjectsOfTypeAll<Transform>().Where(x => x.name.Equals("Enemies")).ToList()[0];
        

        IGNORE_COMMANDS = new DebugCommand<bool> ("ignore_commands", "Player não recebe nenhum input (comando).", "ignore_commands <true/false>",
        (x) => { 
            Player.Instance.IgnoreCommands = x; 
        });

        SET_HP = new DebugCommand<int> ("set_hp", "Faz com que o HP do player seja <value>.", "set_hp <value>",
        (x) => { 
            player.playerInfo.Health = x-1; 
            player.playerInfo.Max_HP = x; 
        });
        SET_MP = new DebugCommand<int> ("set_mp", "Faz com que a MP do player seja <value>.", "set_mp <value>",
        (x) => { 
            player.playerInfo.Mana = x-1; 
            player.playerInfo.Max_MP = x; 
        });
        SET_SP = new DebugCommand<int> ("set_sp", "Faz com que o SP do player seja <value>.", "set_sp <value>",
        (x) => { 
            player.playerInfo.Stamina = x-1; 
            player.playerInfo.Max_SP = x; 
        });

        SHOW_FPS = new DebugCommand<bool> ("show_fps", "Mostra o FPS na tela.", "show_fps <true/false>",
        (x) => { 
            FPSCounter.Instance.showFPS = x; 
        });

        ADJUST_TOOLTIP = new DebugCommand<int, int> ("adjust_tooltip", "Ajusta os limites das dicas flutuantes.", "adjust_tooltip <width> <height>",
        (x, y) => { 
            SkillTooltip.Instance.SetSize(x,y);
        });

        SHOW_INPUTS = new DebugCommand<bool> ("show_inputs", "Mostra a lista de comandos (Ataques) executados.", "show_inputs <true/false>", 
        (x) => {
            DebugInput.ShowPanel(x);
        });

        SET_LEFT_MENU_SIZE = new DebugCommand<int, int> ("set_menu_size", "Ajusta o tamanho do menu do lado esquerdo do inventário."
                                                        , "set_menu_size <width> <height>",
        (x, y) => { 
            MainMenu.Instance.SetMenuSize(x,y);
        });

        ADD_ENEMY = new DebugCommand<string, int> ("add_enemy", "Adiciona inimigos no mapa.", "add_enemy <enemyName> <quantity>",
        (x, y) => { 
            AddEnemy(x,y);
        });

        HELP = new DebugCommand ("help", "Mostra uma lista de todos os comandos existentes.", "help",
        () => { 
            showHelp = !showHelp; 
        });

        commandList = new List<DebugCommandBase>() 
        {
            IGNORE_COMMANDS,
            SET_HP,
            SET_MP,
            SET_SP,
            SHOW_FPS,
            SHOW_INPUTS,
            ADJUST_TOOLTIP,
            SET_LEFT_MENU_SIZE,
            ADD_ENEMY,
            HELP
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)){
            showConsole = !showConsole;
            showHelp = false;
            focus = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && showConsole){
            input = lastCommand;
            focus = true;
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            if (showConsole){
               // ClearInput ();
            }
        }
        // foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
        //     if(Input.GetKeyDown(vKey)){
        //         Debug.Log($"{vKey} was pressed");
        //     }
        // }
    }
    Vector2 scroll;
    private void OnGUI() {
        if (!showConsole)
            return;

        float y = 0f;
        if (showHelp){
            GUI.Box(new Rect(0, y+30, Screen.width, 100), "");
            Rect viewport = new Rect(0, y+ 35f, Screen.width, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y+ 35f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                string label = $"{commandList[i].CommandFormat} - {commandList[i].CommandDescription}";
                Rect labelRect = new Rect(5, 30 + (20 * i), viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            //y += 100;
            GUI.EndScrollView();
        }
        GUI.Box(new Rect(0, y, Screen.width, 30f), "");
        GUI.backgroundColor = new Color(0,0,0,0);
        GUI.SetNextControlName("CommandInput");
        input = GUI.TextField(new Rect(0, y + 5f, Screen.width, 30), input);
        
        if (!input.Equals(""))
            lastCommand = input;

        if (focus){
            GUI.FocusControl("CommandInput");
            focus = false;
        }
        GUI.SetNextControlName("");
        if(Event.current.keyCode == KeyCode.Return){
            ClearInput();
            GUI.FocusControl("");
        }
        // if(Event.current.keyCode == KeyCode.BackQuote){
        //     showConsole = false;
        //     ClearInput();
        //     GUI.FocusControl("");
        // }
    }

    private void ClearInput (){
        HandleInput ();
        input = "";
    }

    private void HandleInput (){
        string[] properties = input.Split(' ');
        foreach (DebugCommandBase commandBase in commandList)
        {
            if (input.Contains(commandBase.CommandId)){
                if (commandBase as DebugCommand != null){
                    (commandBase as DebugCommand).Invoke();
                }else if (commandBase as DebugCommand<int> != null){
                    (commandBase as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }else if (commandBase as DebugCommand<bool> != null && properties[1] != null){
                    (commandBase as DebugCommand<bool>).Invoke(bool.Parse(properties[1]));
                }else if (commandBase as DebugCommand<int, int> != null && properties[1] != null && properties[2] != null){
                    (commandBase as DebugCommand<int, int>).Invoke(int.Parse(properties[1]), int.Parse(properties[2]));
                }else if (commandBase as DebugCommand<string, int> != null && properties[1] != null && properties[2] != null){
                    (commandBase as DebugCommand<string, int>).Invoke(properties[1], int.Parse(properties[2]));
                }

            }
        }
    }

    private void AddEnemy (string enemyName, int quantity){
        Debug.Log($"Procurando {enemyName}");
        GameObject enemy = Resources.Load($"Prefabs/StaticEnemy") as GameObject;

        Vector2 playerPos = Player.Instance.transform.position;

        Collider2D col = new Collider2D();
        float plusStep = step;
        if (enemy != null){
            Enemy e = enemy.GetComponent<Enemy>();
            e.enemyInfo = Resources.Load($"ScriptableObjects/Enemy/{enemyName}") as EnemyInfo;
            if (e.enemyInfo == null){
                Debug.Log($"{enemyName} não existe");
                return;
            }
            for (int i = 0; i < quantity; i++)
            {
                col = new Collider2D();
                do {
                    col = Physics2D.OverlapBox(new Vector2(playerPos.x + step, playerPos.y), new Vector2(size, size), 0f, enemyLayer);
                    Debug.Log($"Collider {col}  pos {new Vector2(playerPos.x + step, playerPos.y)}");
                    if (col != null)
                        step += plusStep;

                    if (Vector2.Distance(playerPos, new Vector2(playerPos.x + step, playerPos.y)) > 1000)
                        break;
                } while (col != null);

                GameObject g = Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
                g.transform.SetParent(enemiesTransform);
                g.transform.position = new Vector2(playerPos.x + step, playerPos.y);

                step += plusStep;
            }
        }
        step = plusStep;
    }

    private void OnDrawGizmosSelected() {
        Vector2 playerPos = Player.Instance.transform.position;
        Gizmos.DrawWireCube(new Vector2(playerPos.x + step, playerPos.y), new Vector2(size, size));
    }
}
