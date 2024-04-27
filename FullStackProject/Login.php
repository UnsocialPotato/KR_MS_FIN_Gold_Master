<?php

$servername = "sim421chickenproject.000webhostapp.com";
$username = "id22059802_ripak1";
$password = "Pjr112792!!";
$dbname = "id22059802_chickenproject";

//variables submitted by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
  if ($conn->connect_error) 
  {
      die("Connection failed: " . $conn->connect_error);
  }

//echo "Connected successfully, now we will show the users.". "<br><br>";

$sql = "SELECT password, id FROM users WHERE username = '". $loginUser. "'";

$result = $conn->query($sql);

  if ($result->num_rows > 0) 
  {
      // output data of each row
      while($row = $result->fetch_assoc()) 
      {
          if($row["password"] == $loginPass)
          {
              echo $row["id"];
              //Get user's data here
              echo $row["username"];

              //Get player info
              echo $row["level"];
              echo $row["coins"];

              //Get inventory

              //Modify player data

              //Update inventory
          }
          else
          {
              echo "Wrong Credentials.";
          }
      }
  } else 
      {
          echo "Username does not exist"; 
      }

$conn->close();

?>