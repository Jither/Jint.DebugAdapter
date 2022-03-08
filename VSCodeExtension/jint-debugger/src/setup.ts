import { DebugConfiguration } from "vscode";

const setup = {
    typeId: "jint"
};

function createDebugConfig(name:string, program:string, request:("attach" | "launch"), connection:("stdinout" | "tcp" | "pipe" | "debugServer")):DebugConfiguration
{
    const result:DebugConfiguration = {
        type: setup.typeId,
        name,
        request,
        program,
        stopOnEntry: true
    };

    switch (connection)
    {
        case "tcp":
            result.port = 4711;
            break;
        case "pipe":
            result.path = "jint";
            break;
        case "debugServer":
            result.debugServer = 4711;
            break;
    }

    return result;
}

function createRunConfig(name:string, program:string):DebugConfiguration
{
    return {
        type: setup.typeId,
        name,
        request: "launch",
        program
    };
}

export default setup;
export {
    createDebugConfig,
    createRunConfig
};
