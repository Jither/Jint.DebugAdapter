import * as vscode from "vscode";
import { WorkspaceFolder, DebugConfiguration, ProviderResult, CancellationToken } from 'vscode';
import { createDebugConfig } from "./setup";

class JintDebugConfigurationProvider implements vscode.DebugConfigurationProvider
{
    resolveDebugConfiguration(folder: WorkspaceFolder | undefined, config: DebugConfiguration, token?: CancellationToken): ProviderResult<DebugConfiguration>
    {
        // If launch.json is missing or empty, make a config for the current file
        if (!config.type && !config.request && !config.name)
        {
            const editor = vscode.window.activeTextEditor;
            if (editor && editor.document.languageId === "javascript")
            {
                const newConfig = createDebugConfig("Debug in Jint (launch)", "$(file)", "launch", "debugServer");
                Object.assign(config, newConfig);
            }
        }

        if (!config.program)
        {
            return vscode.window.showInformationMessage("Could not find a program to debug.").then(_ =>
                {
                    return undefined;
                });
        }

        return config;
    }
}

export default JintDebugConfigurationProvider;