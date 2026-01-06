# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unreal Engine 5 plugin providing MySQL/MariaDB database connectivity via the MariaDB Connector C library. Supports both C++ and Blueprint interfaces.

## Code Style

All new source files must include this copyright header:
```cpp
// Copyright 2026 God Mode Games, LLC. All Rights Reserved.
```

## Build Commands

This is a UE5 plugin, not a standalone project. To build:

1. Place in a UE5 project's `Plugins/MySqlConnect` directory
2. Right-click the `.uproject` file → "Generate Visual Studio Project Files"
3. Open the Visual Studio solution, right-click project → "Project Only" → "Build"

No separate test or lint commands exist—testing is done through the UE5 Editor.

## Architecture

```
Source/
├── MySqlConnect/           # Main plugin module
│   ├── Public/
│   │   ├── MySQLDatabase.h      # Primary API: UMySQLDatabase static methods
│   │   ├── MySQLConnection.h    # Connection wrapper holding MYSQL* pointer
│   │   └── MySQLConnectorStructs.h  # Blueprint-exposed result structs
│   └── Private/
│       └── MySQLDatabase.cpp    # Core implementation (~500 lines)
└── ThirdParty/MySqlConnectLibrary/
    ├── MariaDBConnectorLibs.Build.cs  # Platform-specific library linking
    ├── Win64/                         # Windows x64 (include/ + lib/)
    └── Linux/                         # Linux x64 (include/ + lib/)
```

**Key Classes:**
- `UMySQLDatabase` - Static methods for all database operations (connect, query, table management)
- `UMySQLConnection` - Holds native MYSQL pointer, validates and closes connections
- `FMySQLConnectoreQueryResult` - Query result container (note: typo in struct name is intentional for compatibility)

**Data Flow:**
1. `MySQLInitConnection()` creates connection with configurable timeouts
2. `MySQLConnectorGetData()` executes queries via internal `RunQueryAndGetResults()`
3. Results parsed into `FMySQLConnectorQueryResultRow` arrays with key-value pairs

## Platform Support

- Windows 64-bit (mariadbclient.x64.lib - static library)
- Linux x64 (libmariadbclient.a - static library, requires libssl, libcrypto, zlib, pthread)
- UE5 versions 5.0-5.3 tested
- MySQL 5.0+ and MariaDB compatible

## Known Limitations

- Connection must remain open for game session duration
- `MySQLCloseConnection()` returns false (disabled, needs fix)
- Limited type support: INT and VARCHAR fully supported, others treated as strings
- Port 3306 is hardcoded in connection logic
