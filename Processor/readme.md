pushd Proto && protogen  --csharp_out='./@Generated' **/*.proto && popd
