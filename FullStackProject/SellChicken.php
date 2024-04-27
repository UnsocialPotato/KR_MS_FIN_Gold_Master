<?php

$servername = "sim421chickenproject.000webhostapp.com";
$username = "id22059802_ripak1";
$password = "Pjr112792!!";
$dbname = "id22059802_chickenproject";

    //require "ConectionSettings.php";

    //User submitted variable
    $Id = $_POST["Id"];
    $chickenID = $_POST["chickenID"];
    $userID = $_POST["userID"];

    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

  // Check connection
    if ($conn->connect_error) 
    {
        die("Connection failed: " . $conn->connect_error);
    }

    //First sql
    $sql = "SELECT price FROM chickens WHERE Id = '". $chickenID. "'";

    $result = $conn->query($sql);

    if ($result->num_rows > 0) 
  {
    //Store chicken price
    $chickenPrice = $result->fetch_assoc()["price"];

    //Second Sql (delete chicken)
    $sql2 = "DELETE FROM userschickens WHERE Id = '". $Id . "'";

    $result2 = $conn->query($sql2);
    if($result2)
    {
        //If deleted successfully
        $sql3 = "UPDATE `users` SET `coins` = coins + '". $chickenPrice. "' WHERE `id` = '". $userID. "'";
        $conn->query($sql3);
    }
    else
    {
        echo "error: could not delete chicken";
    }
  } 
  else
  {
    echo "0";
  }

$conn->close();

?>