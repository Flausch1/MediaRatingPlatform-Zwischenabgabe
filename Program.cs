using HttpServerDemo;
using HttpServerDemo.Data;

string connectionString = "Host=localhost;Username=postgres;Password=mysecretpassword;Database=postgres";

// DB konfigurieren
DBInitianlizer dBInitianlizer = new DBInitianlizer(connectionString);
dBInitianlizer.InitDB();

// Server Setup
SimpleHttpServer server = new SimpleHttpServer();

UserService userService = new UserService(new UserRepository(connectionString));

TokenService tokenService = new TokenService(new TokenRepository(connectionString));

HTTPEndpoint userEndpoint = new UserEndpoint("users", userService, tokenService);

server.AddEndpoint(userEndpoint);

MediaService mediaService = new MediaService(new MediaRepository(connectionString));

HTTPEndpoint mediaEndpoint = new MediaEndpoint("media", tokenService, mediaService);
server.AddEndpoint(mediaEndpoint);

// Run Server
await server.Run();
