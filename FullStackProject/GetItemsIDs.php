<?php

$servername = "sim421chickenproject.000webhostapp.com";
$username = "id22059802_ripak1";
$password = "Pjr112792!!";
$dbname = "id22059802_chickenproject";

  //require "ConectionSettings.php";

  //User submitted variable
  $userID = $_POST["userID"];

  // Create connection
  $conn = new mysqli($servername, $username, $password, $dbname);

  // Check connection
    if ($conn->connect_error) 
    {
        die("Connection failed: " . $conn->connect_error);
    }
  
  //echo "Connected successfully, now we will show the users.". "<br><br>";

  $sql = "SELECT ID, itemID FROM usersitems WHERE userID = '". $userID. "'";

  $result = $conn->query($sql);

  if ($result->num_rows > 0) 
  {
    // output data of each row
    $rows = array();
    while($row = $result->fetch_assoc()) 
    {
        $rows[] = $row;
    }
    // after the whole array is created
    echo json_encode($rows);
  } 
  else 
  {
    echo "0"; 
  }

$conn->close();

?>