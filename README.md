# A super simple tcp server and client.
This project here is created so that i dont have to rewrite a tcp connection every time I need one. This project is primarily intended for me and thus no maintenance of the code etc. is expected.


## Send request to client from server.
#### Step 1 - Create method in ClientHandle
Create an void method inside the **ClientHandle** class and give it an parameter of class type **Packet**. This is the method that is going to be called from the server.
```cs
        public static void RequestFromServer(Packet packet)
        {

        }
```
#### Step 2 - Create enum in ServerPackets
Inside **PacketHandler** add a enum value to **ServerPackets** and give it the same name as the method from step one.
```cs
        public enum ServerPackets
        {
            Registration,
            Error,
            RequestFromServer  // <--- This one
        }
```
#### Step 3 - Create method in ServerSend
Inside **ServerSend** create a static void with a similar name as the method written in **ClientHandle** to remember which method is connected to each other and give it a the parameters you want to send, we can also ignore giving it parameters if we don't want to send any data.
```cs
        public static void RequestToClient()
        {
            
        }
```
#### Step 4 - Create method in ServerSend
Still inside the same method, create a new **packet** class and give it the construction parameter of the **enum value** that was created in step two, but cast it into an integer.
```cs
        public static void RequestToClient()
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.RequestFromServer);
        }
```
#### Step 5 - Create method in ServerSend
If we decided to send data to the client, then add the data to the **data** list that lives inside the newly created **Packet** class.
```cs
        public static void RequestToClient(int sendThisInt, string sendThisString)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.RequestFromServer);
            packet.data.Add(sendThisInt);
            packet.data.Add(sendThisString);
        }
```
#### Step 6 - Create method in ServerSend
Now we need to send the **Packet**, to do this we have two methods: **Send** and **SendToAll**. The **Send** method send to a single client meanwhile **SendToAll** sends to all clients. The **Send** method can be given either an **client id** or a **reference to the client socket** to use as a send reference. Both methods also takes the **Packet** as its first parameter.
```cs
        public static void RequestToClient(int sendThisInt, string sendThisString, int sendToClientId)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.RequestFromServer);
            packet.data.Add(sendThisInt);
            packet.data.Add(sendThisString);
            Send(packet, sendToClientId);
        }
```

#### Step 7 - Create dictionary item in PacketHandler
Inside **PacketHandler** add an dictionary item to **clientHandle** with the **enum value** from step two as the key and the **method** from step one as the value.
```cs
        private readonly static Dictionary<ServerPackets, MethodHandler> clientHandle = new Dictionary<ServerPackets, MethodHandler>()
        {
            {ServerPackets.Registration, Connecting.ClientHandle.Registration },
            {ServerPackets.Error, Connecting.ClientHandle.ErrorFromServer },
            {ServerPackets.RequestFromServer, Connecting.ClientHandle.RequestFromServer},  // <--- This one
        };
```

#### Step 8 - Retrive data from packet
If we had passed data to the client we can easily retrieve this data inside the created method inside the **ClientHandle** class. By just accessing the data list that lives inside the Packet parameter.
```cs
        public static void RequestFromServer(Packet packet)
        {
             List<object> dataFromServer = packet.data  // <--- Data stored here

             // Or if we take the examples from step 5.
             int intFromServer = (int)packet.data[0]
             string intFromServer = (string)packet.data[0]
        }
```

#### Step 9 - Call client from server
Finally, to execute the request from the server to the client, just call the method that was created inside **ServerSend**.
```cs
        // Now from anywhere just call
        ServerSend.RequestToClient()
```










## Send request to server from client.


#### Step 1 - Create method in ServerHandle
Create an void method inside the **ServerHandle** class and give it an parameter of class type **Packet**. This is the method that is going to be called from the client.
```cs
        public static void RequestFromClient(Packet packet)
        {

        }
```
#### Step 2 - Create enum in clientPackets
Inside **PacketHandler** add a enum value to **clientPackets** and give it the same name as the method from step one.
```cs
        public enum clientPackets
        {
            ConfirmRegistration,
            RequestFromClient  // <--- This one
        }
```
#### Step 3 - Create method in ClientSend
Inside **ClientSend** create a static void with a similar name as the method written in **ServerHandle** to remember which method is connected to each other and give it a the parameters you want to send, we can also ignore giving it parameters if we don't want to send any data.
```cs
        public static void RequestToServer()
        {
            
        }
```
#### Step 4 - Create method in ClientSend
Still inside the same method, create a new **packet** class and give it the construction parameter of the **enum value** that was created in step two, but cast it into an integer.
```cs
        public static void RequestToServer()
        {
            Packet packet = new Packet((int)PacketHandler.clientPackets.RequestFromClient);
        }
```
#### Step 5 - Create method in ClientSend
If we decided to send data to the server, then add the data to the **data** list that lives inside the newly created **Packet** class.
```cs
        public static void RequestToServer(int sendThisInt, string sendThisString)
        {
            Packet packet = new Packet((int)PacketHandler.clientPackets.RequestFromClient);
            packet.data.Add(sendThisInt);
            packet.data.Add(sendThisString);
        }
```
#### Step 6 - Create method in ClientSend
Now we need to send the **Packet** to the server.
```cs
        public static void RequestToServer(int sendThisInt, string sendThisString)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.RequestFromServer);
            packet.data.Add(sendThisInt);
            packet.data.Add(sendThisString);
            Send(packet);
        }
```

#### Step 7 - Create dictionary item in PacketHandler
Inside **PacketHandler** add an dictionary item to **serverHandle** with the **enum value** from step two as the key and the **method** from step one as the value.
```cs
        private readonly static Dictionary<ClientPackets, MethodHandler> serverHandle = new Dictionary<ClientPackets, MethodHandler>()
        {
            {ClientPackets.ConfirmRegistration, Hosting.ServerHandle.ConfirmRegistration },
            {ClientPackets.RequestFromClient  , Hosting.ServerHandle.RequestFromClient}
        };
```

#### Step 8 - Retrive data from packet
If we had passed data to the server we can easily retrieve this data inside the created method inside the **ServerHandle** class. By just accessing the data list that lives inside the Packet parameter.
```cs
        public static void RequestFromClient(Packet packet)
        {
             List<object> dataFromClient = packet.data  // <--- Data stored here

             // Or if we take the examples from step 5.
             int intFromClient = (int)packet.data[0]
             string intFromClient = (string)packet.data[0]
        }
```

#### Step 9 - Call server from client
Finally, to execute the request from the server to the client, just call the method that was created inside **ClientSend**.
```cs
        // Now from anywhere just call
        ClientSend.RequestToServer()
```


## License
[MIT](https://choosealicense.com/licenses/mit/)
