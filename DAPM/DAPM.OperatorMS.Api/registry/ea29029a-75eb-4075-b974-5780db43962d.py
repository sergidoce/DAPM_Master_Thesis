import argparse


def alpha_miner(input_file, output_file):

    input_file_object = open(input_file, "r")
    content = input_file_object.read()
    input_file_object.close()

    print("Input file content read: " + content)

    new_content = content + "This text was written by the operator"
    
    output_file_object = open(output_file, "w");
    output_file_object.write(new_content)
    output_file_object.close();
        


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("input_file", help="the path to the input CSV file")
    parser.add_argument("output_file", help="the path to the output PNG file")
    args = parser.parse_args()

    alpha_miner(args.input_file, args.output_file)


if __name__ == '__main__':
    main()