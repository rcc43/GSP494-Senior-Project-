using UnityEngine;
using System.Collections.Generic;

public enum enemyType : int { standard, tank, healer, flyer };

public class FormationBlueprint
{
    public int size = 0;
    public List<enemyType> spawnList = new List<enemyType>();

    public FormationBlueprint()
    {
        spawnList = new List<enemyType>();
    }
}


static class FormationLedger
{
    public static List<FormationBlueprint> formationBlueprints;

    public static void Init()
    {
        
        formationBlueprints = new List<FormationBlueprint>();

        FormationBlueprint blueprint = new FormationBlueprint();
        blueprint.size = 3;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.standard);

        formationBlueprints.Add(blueprint);

        blueprint = new FormationBlueprint();
        blueprint.size = 3;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.healer);
        blueprint.spawnList.Add(enemyType.standard);

        formationBlueprints.Add(blueprint);

        blueprint = new FormationBlueprint();
        blueprint.size = 3;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.healer);

        formationBlueprints.Add(blueprint);


        blueprint = new FormationBlueprint();
        blueprint.size = 3;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.tank);
        blueprint.spawnList.Add(enemyType.standard);

        formationBlueprints.Add(blueprint);

        blueprint = new FormationBlueprint();
        blueprint.size = 4;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.healer);
        blueprint.spawnList.Add(enemyType.tank);

        formationBlueprints.Add(blueprint);
        
        blueprint = new FormationBlueprint();
        blueprint.size = 4;
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.standard);
        blueprint.spawnList.Add(enemyType.flyer);
        blueprint.spawnList.Add(enemyType.flyer);

        formationBlueprints.Add(blueprint);
    }
}
