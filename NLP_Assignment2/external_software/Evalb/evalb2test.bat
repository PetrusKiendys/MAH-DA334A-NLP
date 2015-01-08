@REM evaluating gold file against gold file, will produce perfect results
@REM evalb2 in/talbanken-cfg_pos-test.cfg in/talbanken-cfg_pos-test.cfg > out/evalb-out-pos.txt
@REM evalb2 in/talbanken-cfg_dep-test.cfg in/talbanken-cfg_dep-test.cfg > out/evalb-out-dep.txt

@REM evaluating parse output where "(TOP nil)" has been replaced by ""
evalb2 in/talbanken-cfg_pos-test.cfg in/postprocess-out-pos_test.txt > out/evalb-out-pos.txt
evalb2 in/talbanken-cfg_dep-test.cfg in/postprocess-out-dep_test.txt > out/evalb-out-dep.txt