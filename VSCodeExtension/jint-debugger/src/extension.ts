import * as vscode from "vscode";
import JintDebugConfigurationProvider from "./JintDebugConfigurationProvider";
import setup, { createDebugConfig, createRunConfig } from "./setup";

function getTarget(target: vscode.Uri): vscode.Uri
{
	if (!target && vscode.window.activeTextEditor)
	{
		target = vscode.window.activeTextEditor.document.uri;
	}
	return target;
}

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext)
{
	const provider = new JintDebugConfigurationProvider();

	context.subscriptions.push(
		vscode.commands.registerCommand("extension.jint-debugger.getProgram", () => {
			return vscode.window.showInputBox({
				placeHolder: "Please enter the name of a javascript file in the workspace folder.",
				value: "index.js"
			});
		}),

		vscode.commands.registerCommand("extension.jint-debugger.runEditorFile", (resource: vscode.Uri) => {
			const target = getTarget(resource);
			if (target)
			{
				vscode.debug.startDebugging(undefined, createRunConfig("Run file in Jint", target.fsPath),
				{
					noDebug: true
				});
			}
		}),

		vscode.commands.registerCommand("extension.jint-debugger.debugEditorFile", (resource: vscode.Uri) => {
			const target = getTarget(resource);
			if (target)
			{
				vscode.debug.startDebugging(undefined, createDebugConfig("Debug file in Jint (launch)", target.fsPath, "launch", "debugServer"));
			}
		}),

		vscode.debug.registerDebugConfigurationProvider(setup.typeId, provider)
	);
}

// this method is called when your extension is deactivated
export function deactivate() {}
