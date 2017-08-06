math.randomseed(os.time())


function generate_query()
    local query = {}

    seq = math.random(100000, 200000)

    query["fn"] = string.format("Firstname_%s", seq)
    query["ln"] = string.format("Lastname_%s", seq)
    query["rn"] = string.format("ref_%s", seq)

    local keyvals = {}

    for key, val in pairs(query) do
        table.insert(keyvals, string.format("%s=%s", key, val))
    end

    return string.format("/test?%s", table.concat(keyvals, "&"))
end


request = function()
   local path = generate_query()
   return wrk.format(nil, path)
end