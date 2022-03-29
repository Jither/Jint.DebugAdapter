const example = {
    regex: /[a-z0-9]/i,
    date: new Date(),
    boolean: true,
    string: "Here's a string\nwith new-line",
    number: 3.14159265359,
    bigint: 340282366920938463463374607431768211456n,
    null: null,
    undefined: undefined,
    symbol: Symbol("testsymbol"),
    numSymbol: Symbol(1),
    array: [],
    byteArray: new Uint8Array(5),
    intArray: new Int32Array(10),

    get myGetter()
    {
        return "Hello world!";
    }
};

function test()
{
	console.log(count);
    const count = 10;
	console.log(count);
    for (let i = 0; i < count; i++)
    {
        const x = "test";
        console.log(x);
    }
}

function testPause(a, b)
{
    let x = arguments.length;
    const arr = [];
    debugger;
    while (true)
    {
        arr.push(x);
        if (x >= 500)
        {
            break;
        }
        x++;
    }
}

function toInfinityAndBeyond()
{
    let count = 0;
    while (true)
    {
        count++;
    }
}

for (let i = 0; i < 1000; i++)
{
    example.array.push(i);
}
//test();

testPause(5, "test");
toInfinityAndBeyond();