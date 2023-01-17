<?php
	include 'db_connect.php';

	$result = "";
	$notFound = false;
	$missingFields = false;

	if ($_GET['request'] == 'AddSheep')
	{
		if (isset($_GET["Sheep_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepTable` (Sheep_UUID, Sheep_Label, Sheep_Female, Timestamp_Born, Farmer_UUID)"
				." VALUES ('".$_GET["Sheep_UUID"]."','".$_GET["Sheep_Label"]."','".$_GET["Sheep_Female"]."','".$_GET["Timestamp_Born"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddLot')
	{
		if (isset($_GET["Lot_UUID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotTable` (Lot_UUID, Lot_Name, Lot_Surface, Lot_Quality, Lot_Mowed_TS, Lot_State_ID, Farmer_UUID)"
				." VALUES ('".$_GET["Lot_UUID"]."','".$_GET["Lot_Name"]."','".$_GET["Lot_Surface"]."','".$_GET["Lot_Quality"]."','".$_GET["Lot_Mowed_TS"]."','".$_GET["Lot_State_ID"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddPair')
	{
		if (isset($_GET["Pair_Name"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokPairCollection` (Pair_Name, Farmer_UUID, Pair_UUID)"
				." VALUES ('".$_GET["Pair_Name"]."','".$_GET["Farmer_UUID"]."','".$_GET["Pair_UUID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepWeight')
	{
		if (isset($_GET["Sheep_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokWeightTable` (Sheep_ID)"
				." VALUES ('".$_GET["Sheep_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepBreed')
	{
		if (isset($_GET["Breed_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokBreedCollection` (Breed_Name)"
				." VALUES ('".$_GET["Breed_Name"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepFamily')
	{
		if (isset($_GET["Sheep_ID_1"]) and isset($_GET["Sheep_ID_2"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepFamily` (Sheep_ID_1, Sheep_ID_2, Family_Type_ID)"
				." VALUES ('".$_GET["Sheep_ID_1"]."','".$_GET["Sheep_ID_2"]."','".$_GET["Family_Type_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepInfection')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Infection_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepInfection` (Sheep_ID, Infection_ID)"
				." VALUES ('".$_GET["Sheep_ID"]."','".$_GET["Infection_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepPair')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Pair_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepPairs` (Sheep_ID, Pair_ID, Farmer_UUID)"
				." VALUES ('".$_GET["Sheep_ID"]."','".$_GET["Pair_ID"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddSheepMedication')
	{
		if (isset($_GET["Sheep_ID"]) and isset($_GET["Medicine_ID"]) and isset($_GET["Farmer_UUID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokSheepMedicine` (Sheep_ID, Medicine_ID, Dosage, Inject_Timestamp, Farmer_UUID)"
				." VALUES ('".$_GET["Sheep_ID"]."','".$_GET["Medicine_ID"]."','".$_GET["Dosage"]."','".$_GET["Inject_Timestamp"]."','".$_GET["Farmer_UUID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddWormResistence')
	{
		if (isset($_GET["Worm_ID"]) and isset($_GET["Medicine_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokWormResistences` (Worm_ID, Medicine_ID, Resistence_Percentage)"
				." VALUES ('".$_GET["Worm_ID"]."','".$_GET["Medicine_ID"]."','".$_GET["Resistence_Percentage"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddWormWeakness')
	{
		if (isset($_GET["Worm_ID"]) and isset($_GET["Medicine_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokWormWeakness` (Worm_ID, Medicine_ID, Effective_Percentage)"
				." VALUES ('".$_GET["Worm_ID"]."','".$_GET["Medicine_ID"]."','".$_GET["Effective_Percentage"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddLotState')
	{
		if (isset($_GET["Lot_ID"]) and isset($_GET["Lot_State_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotStates` (Lot_ID, Lot_State_ID)"
				." VALUES ('".$_GET["Lot_ID"]."','".$_GET["Lot_State_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddLotPlant')
	{
		if (isset($_GET["Lot_ID"]) and isset($_GET["Plant_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotPlants` (Lot_ID, Plant_ID)"
				." VALUES ('".$_GET["Lot_ID"]."','".$_GET["Plant_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddLotLivestock')
	{
		if (isset($_GET["Lot_ID"]) and isset($_GET["Livestock_ID"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotAnimals` (Lot_ID, Livestock_ID)"
				." VALUES ('".$_GET["Lot_ID"]."','".$_GET["Livestock_ID"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewBreed')
	{
		if (isset($_GET["Breed_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokBreedCollection` (Breed_Name)"
				." VALUES ('".$_GET["Breed_Name"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewFamilyType')
	{
		if (isset($_GET["Family_Type"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokFamilyCollection` (Family_Type)"
				." VALUES ('".$_GET["Family_Type"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewInfection')
	{
		if (isset($_GET["Infection_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokInfectionCollection` (Infection_Name, Infection_Description)"
				." VALUES ('".$_GET["Infection_Name"]."','".$_GET["Infection_Description"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewMedicine')
	{
		if (isset($_GET["Medicine_Latin_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokMedicineCollection` (Medicine_Latin_Name, Medicine_Normal_Name, Medicine_Description)"
				." VALUES ('".$_GET["Medicine_Latin_Name"]."','".$_GET["Medicine_Normal_Name"]."','".$_GET["Medicine_Description"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewLotState')
	{
		if (isset($_GET["State_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLotStateCollection` (State_Name, State_Description)"
				." VALUES ('".$_GET["State_Name"]."','".$_GET["State_Description"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewPlant')
	{
		if (isset($_GET["Plant_Type"]) and isset($_GET["Plant_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokPlantCollection` (Plant_Type, Plant_Name, Plant_Description)"
				." VALUES ('".$_GET["Plant_Type"]."','".$_GET["Plant_Name"]."','".$_GET["Plant_Description"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewLivestock')
	{
		if (isset($_GET["Livestock_Type"]) and isset($_GET["Livestock_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokLivestockCollection` (Livestock_Type, Livestock_Name, Livestock_Description)"
				." VALUES ('".$_GET["Livestock_Type"]."','".$_GET["Livestock_Name"]."','".$_GET["Livestock_Description"]."');");
		}
		else $missingFields = true;
	}
	elseif ($_GET['request'] == 'AddNewWorm')
	{
		if (isset($_GET["Worm_UUID"]) and isset($_GET["Worm_Latin_Name"]))
		{
		    $result = $mysqli->query("INSERT INTO `VerweidklokWormCollection` (Worm_UUID, Worm_Latin_Name, Worm_Normal_Name, Worm_EPG_Danger, Worm_Egg_Description)"
				." VALUES ('".$_GET["Worm_UUID"]."','".$_GET["Worm_Latin_Name"]."','".$_GET["Worm_Normal_Name"]."','".$_GET["Worm_EPG_Danger"]."','".$_GET["Worm_Egg_Description"]."');");
		}
		else $missingFields = true;
	}
	else
	{
		$notFound = true;
		echo("Request not found! Request: ".$_GET['request']);
	}

	if ($missingFields == true)
	{
		$notFound = true;
	    echo("Not all fields set!");
	}

	if ($notFound == false)
	{
		$dataArray = array();
	    while($row = mysqli_fetch_assoc($result))
	    {
	    	$dataArray[] = $row;
	    }
	    echo json_encode($dataArray);
	}

	$mysqli->close();
?>
